import { useEntryPoint } from "../api/context";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { DotNestedKeys } from "../api/helper";
import { useEffect, useState } from "react";
import { Button } from "primereact/button";
import { useElementSize } from "../hooks/element-size-hook";

export function PageProductList() {
    const productList = useEntryPoint("/Product/productList", "get");
    type TProduct = typeof productList["data"]["items"][0];
    const N = (n: DotNestedKeys<TProduct>) => n as string;
    const elementSize = useElementSize<HTMLDivElement>({ offsetHeight: 102 });
    const [query, setQuery] = useState<typeof productList.types.query>({
        RowsPerPage: 10,
        RowsOffset: 0,
    });

    useEffect(() => {
        productList.fetch({ query });
    }, [query]);

    const header = (
        <div className="flex flex-wrap align-items-center justify-content-between gap-2 p-2">
            <span className="text-xl text-900 font-bold">Products: {productList.data?.rowsCount ?? 0} </span>
            <Button icon="pi pi-refresh m-0" rounded raised />
        </div>
    );

    const imageBodyTemplate = (product: TProduct) => {
        return (
            product.details.map(e =>
                <img src={`${e.imageUrl}`} alt={e.imageUrl} className="w-6rem shadow-2 border-round" />)
        )
    };

    return (
        <div className="h-full w-full relative" ref={elementSize.ref}>
            <DataTable
                header={header}
                className="top-0 left-0 absolute w-full"
                value={productList?.data?.items ?? []}
                loading={productList?.isLoading}
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
                <Column header="Image" body={imageBodyTemplate}></Column>
                <Column header="Description" field={N("description")}></Column>
                <Column header="Price" field={N("price")}></Column>
                <Column header="Seller" field={N("seller.name")} ></Column>
                <Column header="Stock" field={N("stock.quantityAvailable")} ></Column>
                <Column header="UpdatedAt" field={N("updatedAt")} ></Column>
            </DataTable>
        </div >
    );
}