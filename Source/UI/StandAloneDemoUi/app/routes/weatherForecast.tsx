import { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { Panel } from "primereact/panel";
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useApi, useEntryPoint } from "~/api/context";
import SubWeatherForecast from "./SubWeatherForecast";


export default function Index() {
    const api = useApi();

    const login = () => {
        api.login({ email: "test", password: "test" })
    }

    useEffect(() => { api.weatherForecast.fetch({}) }, [])

    return (
        <div>

            <Panel header="User Info">
                <p>accessToken: {api.accessToken?.accessToken}</p>
                {(api.userInfo?.data == null ? <p>Not logged in</p>
                    : (Object.keys(api.userInfo?.data!).map(k => <p>{`key: ${k}, value: ${api.userInfo?.data![k]}`}</p>))
                )}
                <Button label="login" onClick={() => login()} ></Button>
            </Panel>

            <h1>Weather Forecast</h1>

            <DataTable value={api.weatherForecast?.data ?? []} tableStyle={{ minWidth: '50rem' }} loading={api.weatherForecast?.isLoading} >
                <Column field="date" header="date"></Column>
                <Column field="summary" header="summary"></Column>
                <Column field="temperatureC" header="temperatureC"></Column>
                <Column field="temperatureF" header="temperatureF"></Column>
            </DataTable>

            <Panel header="Default Preset">
                <SubWeatherForecast ></SubWeatherForecast >
                <Button label="Find" onClick={() => api.weatherForecast.fetch({})} ></Button>
            </Panel>

            <h1>Sub Weather Forecast 2</h1>
            <SubWeatherForecast ></SubWeatherForecast >
        </div >
    );
}