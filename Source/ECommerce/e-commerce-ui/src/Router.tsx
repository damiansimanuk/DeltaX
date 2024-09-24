
import { createBrowserRouter, RouterProvider, Link } from "react-router-dom";
import WeatherForecast from "./pages/WeatherForecast";
import { ProtectedRoute } from "./security/protected-route";
import { DefaultLayout } from "./layout/default-layout";
import { LoginLayout } from "./layout/login-layout";
import { PageLoginForm } from "./security/page-login-form";
import { PageRegisterForm } from "./security/page-register-form";
import { PageProductList } from "./product/page-product-list";
import { ConfigProductForm } from "./product/config-product-form";

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
                    path: "product-list",
                    element: <PageProductList />,
                },
                {
                    path: "config-product",
                    element: <ConfigProductForm />,
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
                    element: <PageLoginForm />,
                },
                {
                    path: "register",
                    element: <PageRegisterForm />,
                },
                {
                    path: "environment",
                    element: <pre> {JSON.stringify(import.meta.env, null, 4)}</pre>,
                },
            ]
        }
    ],
    {
        basename: import.meta.env.BASE_URL
    });

export default function Router() {
    return (
        <RouterProvider router={router} />
    );
}
