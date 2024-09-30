import React, { createContext, useCallback, useContext, useEffect, useState } from 'react'
import { useApi, useWrapFetch } from './Context-';

function createMemoApiContext() {
    let api = useApi()
    const roleList = useMemoWrapFetch(api.context.entryPoint("/security/roleList", 'get').fetch)

    console.log('createMemoApiContext')
    return {
        roleList,
    }
}

type TMemo = ReturnType<typeof createMemoApiContext>

export const ApiMemoContext = createContext<TMemo | null>(null)

export function MemoApiProvider(props: React.PropsWithChildren<{}>) {
    var context = createMemoApiContext()
    return (
        <ApiMemoContext.Provider value={context}>
            {props.children}
        </ApiMemoContext.Provider>
    );
}

export function useMemoApi() {
    const api = useContext(ApiMemoContext)!
    return api
}


export function crateMemoEntryPoint<T extends keyof TMemo>(item: T) {
    return {
        types: {} as TMemo[T],
        use: () => {
            const api = useContext(ApiMemoContext)!
            return api[item]
        },
    }
}


export function useMemoWrapFetch<To, Tr>(fetchFunc: ((o: To) => Promise<Tr>)) {
    const wrap = useWrapFetch(fetchFunc)
    const [options, setOptions] = useState<To>()

    useEffect(() => {
        if (options) {
            wrap.fetch(options)
        }
    }, [options])

    const initialize = useCallback((initialOptions: To) => {
        if (!wrap.done && !wrap.isLoading && !options) {
            setOptions(initialOptions)
        }
    }, [])

    const reset = useCallback(() => {
        wrap.reset()
        setOptions(undefined)
    }, [])

    return {
        done: wrap.done,
        isLoading: wrap.isLoading,
        data: wrap.data,
        errors: wrap.errors,
        reset,
        options,
        setOptions,
        initialize
    }
} 
