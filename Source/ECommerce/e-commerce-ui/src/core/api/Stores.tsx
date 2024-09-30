import { createStoreEntryPoint } from "./ContextZ";


export const roleListStore = createStoreEntryPoint("/security/roleList", "get");
export const userListStore = createStoreEntryPoint("/security/userList", "get");
