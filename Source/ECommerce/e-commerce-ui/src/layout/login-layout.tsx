import { Outlet } from "react-router-dom";

export const LoginLayout: React.FC<{}> = () => {
    return (
        <div className="main-container">
            {/* <header>
                Login Layout
            </header> */}
            <main className="container">
                <Outlet />
            </main>
            <footer>
                &copy; {new Date().getFullYear()} Login Layout
            </footer>
        </div>
    );
};
