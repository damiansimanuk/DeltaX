import { Outlet } from "react-router-dom";



export const DefaultLayout: React.FC<{}> = () => {
    return (
        <div className="grid h-screen grid-rows-[auto_1fr_auto] bg-gray-100 dark:bg-gray-900">
            <header className="bg-white dark:bg-gray-800 py-4 px-4 shadow-md">
                <h1 className="text-3xl font-bold text-gray-800 dark:text-white">
                    Default Layout
                </h1>
            </header>
            <main className="flex-1 p-4 overflow-scroll">
                <Outlet />
            </main>
            <footer className="bg-white dark:bg-gray-800 p-4 text-center">
                <p className="text-sm text-gray-600 dark:text-gray-300">
                    &copy; {new Date().getFullYear()} Default Layout
                </p>
            </footer>
        </div>
    );
};

// grid h-screen grid-rows-[auto_1fr_auto]  bg-red-500
{/* <main className="mx-auto max-w-3xl overflow-scroll"> */ }



// function Header() {
//   return (
//     <div className="flex justify-between bg-yellow-500 p-8">
//       <h1 className="text-4xl uppercase">HeaderText</h1>
//       <input
//         className="w-36 rounded-full bg-red-50 px-4 py-2 transition-all focus:w-60"
//         placeholder="Search.."
//       />
//     </div>
//   );
// }


// function Footer() {
//   return (
//     <div className="h-14 flex justify-center items-center bg-slate-700 p-4">
//       <h1 className="text-white">This is Footer</h1>
//     </div>
//   );
// }