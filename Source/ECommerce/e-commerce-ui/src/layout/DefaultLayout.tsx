import { Link, Outlet, useNavigate } from "react-router-dom";
import { Spinner } from "./Spinner";
import { Button } from "primereact/button";
import { logout, userInfoStore } from "../core/api/Shared";

export const DefaultLayout: React.FC<{}> = () => {
    const userInfo = userInfoStore.use()
    const navigate = useNavigate()
    const isAnonymous = !userInfo.data?.email

    const goToLogin = () => {
        navigate("security/login")
    }

    return (
        <div className="main-container">
            <header className="md:flex align-items-center ">
                <div className="flex align-items-center">
                    <span>isLoading:{`${userInfo.isLoading}`} </span>
                    <span>done:{`${userInfo.done}`} </span>
                    <span>isAnonymous:{`${isAnonymous}`} </span>
                    <span>User:{userInfo.data?.userName ?? 'Anónimo'} </span>
                </div>
                <div className="flex flex-1 align-items-center">
                    <span> <Link to="/">Home</Link> </span>
                </div>
                <div className="flex align-items-center">
                    <span className="p-2">{userInfo.data?.userName ?? 'Anónimo'} </span>
                    {isAnonymous
                        ? <Button label="login" className="mr-1 p-2" icon="pi pi-sign-in" iconPos="right" onClick={goToLogin} />
                        : <Button label="logout" className="mr-1 p-2" icon="pi pi-power-off" iconPos="right" onClick={() => logout()} />
                    }
                </div>
            </header>

            <main className="container relative p-2">
                {/* {(!userInfo.done || userInfo.isLoading ) && <Spinner loading className=" " />} */}
                <Spinner loading={(!userInfo.done || userInfo.isLoading)} className="absolute" />

                {userInfo.done && <Outlet />}
            </main>

            <footer>
                &copy; {new Date().getFullYear()} E Commerce Demo
            </footer>
        </div>
    );
}; 
