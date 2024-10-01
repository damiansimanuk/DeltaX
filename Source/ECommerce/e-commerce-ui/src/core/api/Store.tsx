import { ApiRequest, HttpError, Paths } from './request/ApiRequest'
import { OmitNever, RequestTypeBuilder } from './request/RequestBuilder';
import { create, useStore } from 'zustand'

export const useCookies = true
export const baseUrl = "http://localhost:5299"

type Url = keyof Paths
type Method<U extends Url> = keyof Omit<OmitNever<Paths[U]>, "parameters">
type TStoreFetch<To, Td> = {
    done: boolean,
    isLoading: boolean,
    data: Td | undefined,
    options: To | undefined,
    errors: { code: string, message: string }[] | undefined,
    fetch: (o: To) => Promise<Td>,
    initialize: (o: To) => void,
    setOptions: (o: To) => void,
    reset: () => void
}

export const TypeBuilder = RequestTypeBuilder<Paths>();

export const api = ApiRequest(baseUrl, useCookies)

export function createStoreEntryPoint<U extends Url, M extends Method<U>>(url: U, method: M) {
    console.log("createStoreEntryPoint")

    const types = TypeBuilder(url, method);
    const entryPoint = api.entryPoint(url, method)
    const store = createStoreFetch(entryPoint.fetch)

    return {
        types,
        store,
        getState: store.getState,
        use: () => {
            console.log("createStoreEntryPoint use")
            return useStore(store)
        }
    }
}

export function createStoreFetch<To, Tr>(fetchFunc: ((o: To) => Promise<Tr>)) {
    console.log("createStoreFetch")
    const store = create<TStoreFetch<To, Tr>>((set, get) => ({
        done: false,
        isLoading: false,
        data: undefined,
        errors: undefined,
        options: undefined,
        fetch: (options: To) => {
            set({ isLoading: true, })
            return fetchFunc(options)
                .then(r => {
                    console.log('fetchFunc ok', options, r)
                    set({ data: r, errors: undefined, })
                    return r
                }).catch((e: Error | HttpError) => {
                    console.log('fetchFunc error', options, e)
                    set({ data: undefined })
                    if (e instanceof HttpError) {
                        set({ errors: e.errors })
                    }
                    else {
                        const errors = [{ code: "Message", message: `${e}` }]
                        set({ errors: errors })
                    }
                    throw e
                }).finally(() => {
                    set({ isLoading: false, done: true })
                })
        },
        initialize: (initialOptions: To) => {
            const state = get()
            if (!state.done && !state.isLoading && !state.options) {
                set({ options: initialOptions })
            }
        },
        setOptions: (opt: To) => {
            set({ options: opt })
        },
        reset: () => {
            set({
                done: false,
                isLoading: false,
                data: undefined,
                errors: undefined
            })
        }
    }))

    store.subscribe((state, prevState) => {
        if (state?.options && state?.options !== prevState?.options) {
            console.log("store.fetch options", state.options)
            state.fetch(state.options)
        }
    })

    return store;
}


