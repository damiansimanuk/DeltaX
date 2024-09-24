import { Outlet } from "react-router-dom";

export const DefaultLayout: React.FC<{}> = () => {
    return (
        <div className="main-container">
            <header>
                Default Layout
            </header>
            <main className="container">
                <Outlet />
            </main>
            <footer>
                &copy; {new Date().getFullYear()} E Commerce Demo
            </footer>
        </div>
    );
}; 
