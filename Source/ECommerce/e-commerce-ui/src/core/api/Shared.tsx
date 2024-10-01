import { createStoreEntryPoint, api, useCookies } from "./Store";


export const roleListStore = createStoreEntryPoint("/security/roleList", "get");
export const userListStore = createStoreEntryPoint("/security/userList", "get");

const logoutStore = createStoreEntryPoint("/security/logout", "post")
export const userInfoStore = createStoreEntryPoint("/security/userInfo", "get")
export const loginStore = createStoreEntryPoint("/security/login", "post")

userInfoStore.getState().fetch({})

loginStore.store.subscribe((state, prevState) => {
    if (state.done && state.data?.accessToken !== prevState.data?.accessToken) {
        api.setAccessToken(state.data.accessToken ?? "")
        userInfoStore.getState().fetch({})
    }
    if (useCookies && state.done && !state.isLoading && prevState.isLoading) {
        userInfoStore.getState().fetch({})
    }
})

export function logout() {
    logoutStore.getState()
        .fetch({ query: { returnUrl: window.location.pathname } })
        .finally(() => {
            loginStore.getState().reset()
            userInfoStore.getState().reset()
            userInfoStore.getState().fetch({})
        })
}
