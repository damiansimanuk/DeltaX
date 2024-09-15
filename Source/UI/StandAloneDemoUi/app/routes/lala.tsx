import { useState } from "react";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { Panel } from "primereact/panel";

export default function Home() {
    const [count, setCount] = useState(0);

    return (
        <>
            <div
                className="
                m-auto
          bg-white
          dark:bg-gray-800
          p-10
          mt-10
          rounded-xl
          flex flex-col
          gap-8
          max-w-3xl
        "
            >
                <h1 className="text-4xl text-black dark:text-white font-bold text-center">
                    Tailwind CSS + PrimeReact
                </h1>
                <Panel header="Default Preset">
                    <p>
                        First panel component uses the global pass through preset from the
                        Tailwind CSS based implementation of PrimeOne Design 2023.
                    </p>
                </Panel>

                <Panel >
                    <p>
                        Second panel overrides the header section with custom a custom style.
                    </p>
                </Panel>

            </div>
        </>
    );
}