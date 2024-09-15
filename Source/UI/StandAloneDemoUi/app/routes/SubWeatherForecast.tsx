import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useApi, useEntryPoint } from "~/api/context";

export default function SubWeatherForecast() {
    const api = useApi();

    return (
        <>
            <DataTable value={api.weatherForecast.data ?? []} tableStyle={{ minWidth: '50rem' }} loading={api.weatherForecast?.isLoading} >
                <Column field="date" header="date"></Column>
                <Column field="summary" header="summary"></Column>
                <Column field="temperatureC" header="temperatureC"></Column>
                <Column field="temperatureF" header="temperatureF"></Column>
            </DataTable>
        </>
    );
}