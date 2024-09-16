
import { createBrowserRouter, RouterProvider, Link } from "react-router-dom";
import WeatherForecast from "./pages/WeatherForecast"; 
import { ProtectedRoute } from "./security/protected-route";


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
