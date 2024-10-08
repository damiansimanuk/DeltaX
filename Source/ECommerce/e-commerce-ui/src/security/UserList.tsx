import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { DotNestedKeys } from "../core/api/Helper";
import { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { useElementSize } from "../core/hooks/useElementSize";
import { ConfigUserDialog } from "./ConfigUserForm";
import { Tag } from "primereact/tag";
import { userListStore } from "../core/api/Shared";

export function UserList() {
    type TItem = typeof list["data"]["items"][0];
    const list = userListStore.use();
    const N = (n: DotNestedKeys<TItem>) => n as string;
    const elementSize = useElementSize<HTMLDivElement>({ offsetHeight: 100 });
    const [editItem, setEditItem] = useState<TItem>();

    useEffect(() => {
        list.initialize({ query: { RowsPerPage: 10000 } }, 600);
    }, []);

    const header = (
        <div className="flex flex-wrap align-items-center justify-content-between gap-2 p-2 py-0 h-3rem">
            <span className="text-xl text-900 font-bold">Lista de usuarios</span>
        </div>
    );

    const onCloseDialog = (success: boolean) => {
        setEditItem(undefined)
        if (success) {
            list.setOptions({ ...list.options });
        }
    }

    return (
        <div className="h-full w-full" >

            <ConfigUserDialog onSuccess={() => onCloseDialog(true)} onHide={() => onCloseDialog(false)} item={editItem} />

            <div className="h-full w-full relative" ref={elementSize.ref}>
                <DataTable
                    header={header}
                    className="top-0 left-0 absolute w-full"
                    value={list?.data?.items ?? []}
                    loading={list?.isLoading}
                    scrollable
                    scrollHeight={elementSize.height}
                    rowsPerPageOptions={[5, 10, 25, 50, 100]}
                    paginator
                    showGridlines
                    rows={25}
                >
                    <Column header="Name" field={N("userName")} frozen
                        style={{ width: '1rem' }}
                        className="nowrap font-bold p-0 px-2 bg-bluegray-900 border-right-1"
                        headerClassName="bg-bluegray-900 border-right-1"
                    />
                    <Column header="Email" field={N("email")}
                        style={{ minWidth: '300px' }}
                        className='text-cyan-200 border-left-none p-0 px-2'
                        headerClassName="border-left-none"
                    />
                    <Column header="Full Name" field={N("fullName")}
                        style={{ minWidth: '200px' }}
                        className="p-0 px-2 nowrap"
                    />
                    <Column header="Roles"
                        style={{ minWidth: '300px' }}
                        className="m-0 p-0 px-2"
                        body={(r: TItem) => <>
                            {r.roles.map(e => (<Tag key={e} value={e} className="nowrap mr-1"> </Tag>))}
                        </>}
                    />
                    <Column header="Phone" field={N("phoneNumber")}
                        style={{ minWidth: '100px' }}
                        className="m-0 p-0 px-2 nowrap"
                    />
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