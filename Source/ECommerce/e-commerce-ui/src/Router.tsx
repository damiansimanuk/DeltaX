
import { createBrowserRouter, RouterProvider, Link } from "react-router-dom";
import { ProtectedRoute } from "./security/ProtectedRoute";
import { DefaultLayout } from "./layout/DefaultLayout";
import { LoginLayout } from "./layout/LoginLayout";
import { PageLoginForm } from "./security/PageLoginForm";
import { PageRegisterForm } from "./security/PageRegisterForm";
import { ProductList } from "./product/ProductList";
import { ConfigProductForm } from "./product/ConfigProductForm";
import { ConfigSellerForm } from "./product/ConfigSellerForm";
import { SellerList } from "./product/SellerList";
import { WeatherForecast } from "./pages/WeatherForecast";
import { UserList } from "./security/UserList";
import { RoleList } from "./security/RoleList";
import { PageResetPasswordForm } from "./security/PageResetPasswordForm";

export const router = createBrowserRouter(
    [
        {
            element: <DefaultLayout />,
            children: [
                {
                    path: "/",
                    element: (
                        <div className="grid gap-4 m-0">
                            <h1>Hello World</h1>
                            <h1 className="text-3xl font-bold underline">
                                Hello world!
                            </h1>
                            <Link to="about">About Us</Link>
                            <Link to="protected">protected</Link>
                            <Link to="landing">landing</Link>
                            <Link to="WeatherForecast">WeatherForecast</Link>
                            <Link to="security/login">login</Link>
                            <Link to="security/role-list">roles</Link>
                            <Link to="security/user-list">users</Link>
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
            element: <DefaultLayout />,
            path: "product",
            children: [
                {
                    path: "product-list",
                    element: <ProductList />,
                },
                {
                    path: "seller-list",
                    element: <SellerList />,
                },
            ]
        },
        {
            element: <DefaultLayout />,
            path: "security",
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
                    path: "resetPassword",
                    element: <PageResetPasswordForm />,
                },
                {
                    path: "user-list",
                    element: <UserList />,
                },
                {
                    path: "role-list",
                    element: <RoleList />,
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
