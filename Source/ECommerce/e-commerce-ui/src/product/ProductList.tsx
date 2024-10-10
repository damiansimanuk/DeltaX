import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { DotNestedKeys } from "../core/api/Helper";
import { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { useElementSize } from "../core/hooks/useElementSize";
import { createStoreEntryPoint, TypeBuilder } from "../core/api/Store";
import { Dialog } from "primereact/dialog";
import { ConfigProductForm } from "./ConfigProductForm";
import { Spinner } from "../layout/Spinner";

const productListStore = createStoreEntryPoint("/api/v1/Product/productList", "get");
const productItemStore = createStoreEntryPoint("/api/v1/Product/product/{productId}", "get");
type TItem = typeof productListStore.types.result.items[0];
const productType = TypeBuilder("/api/v1/Product/product", "post");
type TEditItem = typeof productType.body;
const N = (n: DotNestedKeys<TItem>) => n as string;

export function ProductList() {
    const productList = productListStore.use();
    const productItem = productItemStore.use();
    const [editItem, setEditItem] = useState<TEditItem>();
    const elementSize = useElementSize<HTMLDivElement>({ offsetHeight: 102 });
    const [query, setQuery] = useState<typeof productListStore.types.query>({
        RowsPerPage: 10,
        RowsOffset: 0,
    });

    useEffect(() => {
        productList.setOptions({ query });
    }, [query]);

    const onEditItemSuccess = () => {
        setEditItem(undefined)
        productList.setOptions({ ...productList.options });
    }

    const getAndEditItem = (productId: number) => {
        productItem.fetch({ path: { productId } }).then(r =>
            setEditItem({
                ...r,
                productId: r.id,
                categories: r.categories?.map(c => c.name) ?? [],
                sellerId: r.seller?.id,
            }));
    }

    const header = (
        <div className="flex flex-wrap align-items-center justify-content-between gap-2 p-2">
            <span className="text-xl text-900 font-bold">Products: {productList.data?.rowsCount ?? 0} </span>
            <Button icon="pi pi-plus" rounded text className="m-0 mr-2 p-0" onClick={() => setEditItem({})} />
        </div>
    );

    return (
        <div className="h-full w-full" >
            <Dialog
                maximizable
                visible={!!editItem}
                header={(editItem?.name ? `Config product ${editItem?.name}` : 'Create a new product')}
                onHide={() => setEditItem(undefined)}
                style={{ width: '640px' }} 
            >
                <ConfigProductForm
                    item={editItem}
                    onSuccess={onEditItemSuccess}
                />
            </Dialog>

            <div className="h-full w-full relative" ref={elementSize.ref}>

                <Spinner loading={productList?.isLoading || productItem.isLoading} />

                <DataTable
                    header={header}
                    className="top-0 left-0 absolute w-full"
                    value={productList?.data?.items ?? []}
                    scrollHeight={elementSize.height}
                    paginator
                    rows={query.RowsPerPage}
                    first={query.RowsOffset}
                    rowsPerPageOptions={[10, 25, 50, 100]}
                    totalRecords={productList?.data?.rowsCount}
                    onPage={(e) => setQuery({ RowsOffset: e.first, RowsPerPage: e.rows })}
                >
                    <Column header="Id" field={N("id")} ></Column>
                    <Column header="Name" field={N("name")} ></Column>
                    <Column header="Description" field={N("description")}></Column>
                    <Column header="Seller" field={N("seller.name")} ></Column>
                    <Column header="Price" field={N("stock.price")} ></Column>
                    <Column header="Stock" field={N("stock.quantityAvailable")} ></Column>
                    <Column header="UpdatedAt" field={N("updatedAt")} ></Column>
                    <Column header="Actions" frozen alignFrozen="right" align="center"
                        style={{ width: '1rem' }}
                        className="m-0 p-0 py-1 bg-bluegray-900"
                        headerClassName="bg-bluegray-900"
                        body={(r: TItem) => <>
                            <Button icon="pi pi-pencil" rounded text className="m-0 p-0 shadow-none" onClick={() => getAndEditItem(r.id)} />
                        </>}
                    />
                </DataTable>
            </div >
        </div >
    );
}