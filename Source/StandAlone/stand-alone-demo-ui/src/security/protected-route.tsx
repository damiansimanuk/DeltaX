import { Navigate } from "react-router-dom";
import { useApi } from "../api/context";

export function ProtectedRoute(props: React.PropsWithChildren<{ redirectPath?: string | undefined, permissions?: string[] | undefined }>) {
    var api = useApi();

    if (props.permissions && !api.userInfo.data) {
        return <Navigate to={props.redirectPath ?? "/landing"} replace />;
    }

    return props.children;
}
