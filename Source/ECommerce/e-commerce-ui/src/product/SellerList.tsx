import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { DotNestedKeys } from "../core/api/Helper";
import { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { useElementSize } from "../core/hooks/useElementSize";
import { createStoreEntryPoint } from "../core/api/Store";

const listStore = createStoreEntryPoint("/api/v1/Product/sellerList", "get");

export function SellerList() {
    type TItem = typeof listStore.types.result.items[0];
    const list = listStore.use();
    const N = (n: DotNestedKeys<TItem>) => n as string;
    const elementSize = useElementSize<HTMLDivElement>({ offsetHeight: 102 });
    const [query, setQuery] = useState<typeof listStore.types.query>({
        RowsPerPage: 10,
        RowsOffset: 0,
    });

    useEffect(() => {
        list.fetch({ query });
    }, [query]);

    const header = (
        <div className="flex flex-wrap align-items-center justify-content-between gap-2 p-2">
            <span className="text-xl text-900 font-bold">Products: {list.data?.rowsCount ?? 0} </span>
            <Button icon="pi pi-refresh m-0" rounded raised />
        </div>
    );

    return (
        <div className="h-full w-full relative" ref={elementSize.ref}>
            <DataTable
                header={header}
                className="top-0 left-0 absolute w-full"
                value={list?.data?.items ?? []}
                loading={list?.isLoading}
                scrollHeight={elementSize.height}
                paginator
                rows={query.RowsPerPage}
                first={query.RowsOffset}
                rowsPerPageOptions={[10, 25, 50, 100]}
                totalRecords={list?.data?.rowsCount}
                onPage={(e) => setQuery({ RowsOffset: e.first, RowsPerPage: e.rows })}
            >
                <Column header="Id" field={N("id")} ></Column>
                <Column header="Name" field={N("name")} ></Column>
                <Column header="Description" field={N("email")}></Column>
                <Column header="Price" field={N("phoneNumber")}></Column>
                <Column header="UpdatedAt" field={N("updatedAt")} ></Column>
            </DataTable>
        </div >
    );
}