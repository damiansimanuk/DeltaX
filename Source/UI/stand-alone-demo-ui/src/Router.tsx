
import { createBrowserRouter, RouterProvider, Link, Navigate } from "react-router-dom";
import WeatherForecast from "./pages/WeatherForecast";
import { useApi } from "./api/context";


export function ProtectedRoute(props: React.PropsWithChildren<{ redirectPath?: string | undefined, permissions?: string[] | undefined }>) {
    var api = useApi();

    if (props.permissions && !api.userInfo.data) {
        return <Navigate to={props.redirectPath ?? "/landing"} replace />;
    }
    return props.children;
}

export const router = createBrowserRouter(
    [
        {
            path: "/",
            element: (
                <div>
                    <h1>Hello World</h1>
                    <Link to="about">About Us</Link>
                    <Link to="protected">protected</Link>
                    <Link to="landing">landing</Link>
                    <Link to="WeatherForecast">WeatherForecast</Link>
                </div>
            ),
        },
        {
            path: "about",
            element: <div>About</div>,
        },
        {
            path: "WeatherForecast",
            element: <WeatherForecast />,
        },
        {
            path: "protected",
            element: (<ProtectedRoute permissions={["admin"]}>Protected</ProtectedRoute>),
        },
        {
            path: "landing",
            element: <div>landing</div>,
        },
    ],
    {
        basename: '/demo/ui/'
    });

export default function Router() {
    return (
        <RouterProvider router={router} />
    );
}
