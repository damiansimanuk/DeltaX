import { Navigate } from "react-router-dom";
import { userInfoStore } from "../core/api/Shared";

export function ProtectedRoute(props: React.PropsWithChildren<{ redirectPath?: string | undefined, permissions?: string[] | undefined }>) {
    const userInfo = userInfoStore.use();

    if (props.permissions && !userInfo.data) {
        return <Navigate to={props.redirectPath ?? "/landing"} replace />;
    }

    return props.children;
}
