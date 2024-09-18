
import { createBrowserRouter, RouterProvider, Link } from "react-router-dom";
import WeatherForecast from "./pages/WeatherForecast";
import { ProtectedRoute } from "./security/protected-route";
import { DefaultLayout } from "./layout/default-layout";
import { LoginLayout } from "./layout/login-layout";
import LoginForm from "./pages/LoginForm";


export const router = createBrowserRouter(
    [
        {
            element: <DefaultLayout />,
            children: [
                {
                    path: "/",
                    element: (
                        <div>
                            <h1>Hello World</h1>
                            <h1 className="text-3xl font-bold underline">
                                Hello world!
                            </h1>
                            <Link to="about">About Us</Link>
                            <Link to="protected">protected</Link>
                            <Link to="landing">landing</Link>
                            <Link to="WeatherForecast">WeatherForecast</Link>
                        </div>
                    ),
                },
                {
                    path: "WeatherForecast",
                    element: <WeatherForecast />,
                },
                {
                    path: "protected",
                    element: (<ProtectedRoute permissions={["admin"]}>Protected</ProtectedRoute>),
                },
            ]
        },
        {
            element: <LoginLayout />,
            children: [
                {
                    path: "about",
                    element: <div>About</div>,
                },
                {
                    path: "WeatherForecast2",
                    element: <WeatherForecast />,
                },
                {
                    path: "landing",
                    element: <div>landing</div>,
                },
                {
                    path: "login",
                    element: <LoginForm />,
                },
            ]
        }
    ],
    {
        basename: '/demo/ui/'
    });

export default function Router() {
    return (
        <RouterProvider router={router} />
    );
}
