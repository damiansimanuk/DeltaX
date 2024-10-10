import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { DotNestedKeys } from "../core/api/Helper";
import { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { useElementSize } from "../core/hooks/useElementSize";
import { createStoreEntryPoint } from "../core/api/Store";
import { Dialog } from "primereact/dialog";
import { ConfigSellerForm } from "./ConfigSellerForm";
import { Spinner } from "../layout/Spinner";

const listStore = createStoreEntryPoint("/api/v1/Product/sellerList", "get");
type TItem = typeof listStore.types.result.items[0];
const N = (n: DotNestedKeys<TItem>) => n as string;

export function SellerList() {
    const list = listStore.use();
    const elementSize = useElementSize<HTMLDivElement>({ offsetHeight: 102 });
    const [editItem, setEditItem] = useState<TItem>();
    const [query, setQuery] = useState<typeof listStore.types.query>({
        RowsPerPage: 10,
        RowsOffset: 0,
    });

    useEffect(() => {
        list.setOptions({ query });
    }, [query]);

    const onEditItemSuccess = () => {
        setEditItem(undefined)
        list.setOptions({ ...list.options });
    }

    const header = (
        <div className="flex flex-wrap align-items-center justify-content-between gap-2 p-2">
            <span className="text-xl text-900 font-bold">Products: {list.data?.rowsCount ?? 0} </span>
            <Button icon="pi pi-plus" rounded text className="m-0 mr-2 p-0" onClick={() => setEditItem({})} />
        </div>
    );

    return (
        <div className="h-full w-full" >
            <Dialog
                visible={!!editItem}
                header={(editItem?.name ? `Config seller ${editItem?.name}` : 'Create a new seller')}
                onHide={() => setEditItem(undefined)}
                style={{ width: '640px' }} >
                <ConfigSellerForm
                    item={{ ...editItem, sellerId: editItem?.id }}
                    onSuccess={onEditItemSuccess}
                />
            </Dialog>

            <div className="h-full w-full relative " ref={elementSize.ref}>

                <Spinner loading={list?.isLoading} />

                <DataTable
                    header={header}
                    className="top-0 left-0 absolute w-full"
                    value={list?.data?.items ?? []}
                    scrollHeight={elementSize.height}
                    paginator
                    rowsPerPageOptions={[10, 25, 50, 100]}
                    rows={query.RowsPerPage}
                    first={query.RowsOffset}
                    totalRecords={list?.data?.rowsCount}
                    onPage={(e) => setQuery({ RowsOffset: e.first, RowsPerPage: e.rows })}
                >
                    <Column header="Id" field={N("id")} />
                    <Column header="Name" field={N("name")} />
                    <Column header="Description" field={N("email")} />
                    <Column header="Phone" field={N("phoneNumber")} />
                    <Column header="UpdatedAt" field={N("updatedAt")} />
                    <Column header="Actions" frozen alignFrozen="right" align="center"
                        style={{ width: '1rem' }}
                        className="m-0 p-0 py-1 bg-bluegray-900"
                        headerClassName="bg-bluegray-900"
                        body={(r) => <>
                            <Button icon="pi pi-pencil" rounded text className="m-0 p-0 shadow-none" onClick={() => setEditItem(r)} />
                        </>}
                    />
                </DataTable>
            </div >
        </div >
    );
}