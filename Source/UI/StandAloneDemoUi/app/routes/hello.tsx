import { Link } from "@remix-run/react";
import { Button } from 'primereact/button';



export default function () {
    return (
        <div>
            Hola mundo ok

            go to <Link to="/">home</Link>

            <div>
                <Button label="Submit" />
                <Button label="Primary" />
                <Button label="Secondary" severity="secondary" />
                <Button label="Success" severity="success" />
                <Button label="Info" severity="info" />
                <Button label="Warning" severity="warning" />
                <Button label="Help" severity="help" />
                <Button label="Danger" severity="danger" />
            </div>
        </div>
    );
}
