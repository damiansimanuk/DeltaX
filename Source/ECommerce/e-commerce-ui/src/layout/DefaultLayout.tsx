import { Link, Outlet } from "react-router-dom";
import { Spinner } from "./Spinner";
import { Button } from "primereact/button";
import { useState } from "react";
import { userInfoStore } from "../core/api/ContextZ";

export const DefaultLayout: React.FC<{}> = () => {
    const userInfo = userInfoStore.use()
    const [test, setTest] = useState(false)
    const isAnonymous = userInfo.done && !userInfo.data?.email

    return (
        <div className="main-container">
            <header className="flex align-items-center align-content-center">
                <span>isLoading:{`${userInfo.isLoading}`} </span>
                <span>done:{`${userInfo.done}`} </span>
                <span>isAnonymous:{`${isAnonymous}`} </span>
                <span>User:{userInfo.data?.email ?? 'Anónimo'} </span>
                <span>test:{test ? 'true' : 'false'} </span>
                <span> <Link to="/">Home</Link> </span>

                <Button label="test" onClick={() => setTest(p => !p)} />
                {/* <Button label="logout" onClick={() => auth.logout({ returnUrl: window.location.pathname })} /> */}
            </header>

            <main className="container relative p-2">
                {/* {(!userInfo.done || userInfo.isLoading ) && <Spinner loading className=" " />} */}
                <Spinner loading={(!userInfo.done || userInfo.isLoading || test)} className="absolute" />

                {userInfo.done && <Outlet />}
            </main>

            <footer>
                &copy; {new Date().getFullYear()} E Commerce Demo
            </footer>
        </div>
    );
}; 
