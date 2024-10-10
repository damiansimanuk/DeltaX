import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { useForm, useController, useFieldArray, Controller } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { AutoComplete, AutoCompleteCompleteEvent } from 'primereact/autocomplete';
import { useEffect, useRef, useState } from 'react';
import { Panel } from 'primereact/panel';
import { InputFile } from '../components/InputFile';
import { InputTextarea } from 'primereact/inputtextarea';
import { createStoreEntryPoint } from '../core/api/Store';

const requestSellerListStore = createStoreEntryPoint("/api/v1/Product/sellerList", "get");
const requestStore = createStoreEntryPoint("/api/v1/Product/product", "post");
type TForm = typeof requestStore.types.body;
type TSeller = { id: number, name: string }

const categoriesCatalog = [
    'New York',
    'Rome',
    'London',
    'Istanbul',
    'Paris',
];

export function ConfigProductForm({ item, onSuccess, onError }: {
    item: TForm,
    onSuccess?: () => void
    onError?: () => void
}) {
    const request = requestStore.use();
    const sellerList = requestSellerListStore.use();
    const [availableCategories, setAvailableCategories] = useState(categoriesCatalog);
    const [availableSeller, setAvailableSeller] = useState<TSeller[]>([]);
    const panelRef = useRef(null);
    const { control, handleSubmit, getFieldState } = useForm<TForm & { seller: TSeller }>({ defaultValues: item })
    const details = useFieldArray({ control, name: "details" })


    const seller = useController({
        control,
        name: 'seller',
        rules: {
            required: {
                value: true,
                message: 'seller is required'
            },
        }
    })

    const name = useController({
        control,
        name: 'name',
        rules: {
            required: {
                value: true,
                message: 'Name is required'
            },
        }
    })

    const description = useController({
        control,
        name: 'description',
        rules: {
            required: {
                value: true,
                message: 'Description is required'
            },
        }
    })

    const categories = useController({
        control,
        name: 'categories',
        rules: {
            validate: (value) => value?.length >= 1 || 'Category is required',
        }
    })

    useEffect(() => {
        request.reset();
        sellerList.initialize({ query: { RowsPerPage: 10000 } });
    }, [])

    const onSubmit = handleSubmit(data => {
        console.log(JSON.stringify(data));
        const categories = data.categories || [];
        const details = data.details || [];
        const body = { ...data, sellerId: data.seller?.id, categories, details }
        console.log({ body })
        request.fetch({ body })
            .then(() => onSuccess?.())
            .catch(() => onError?.())
    })

    const onAddDetail = handleSubmit(() => {
        details.append({ description: "", imageUrl: '' })
        panelRef.current.expand()
    })

    const onSearchSheller = (event: AutoCompleteCompleteEvent) => {
        setTimeout(() => {
            let filtered = sellerList.data?.items.map(e => ({ id: e.id, name: e.name })) ?? []
            if (event.query.trim().length) {
                let query = event.query.toLowerCase()
                filtered = filtered.filter(c => c.name.toLowerCase().startsWith(query));
            }
            setAvailableSeller(filtered);
        }, 50);
    }

    const onSearchCategories = (event: AutoCompleteCompleteEvent) => {
        setTimeout(() => {
            if (!event.query.trim().length) {
                setAvailableCategories(categoriesCatalog);
            }
            else {
                let query = event.query.toLowerCase()
                let filtered = categoriesCatalog.filter(c => c.toLowerCase().startsWith(query));
                filtered.push(event.query);
                setAvailableCategories(filtered);
            }
        }, 50);
    }

    const iconAddDetail = <>
        <Button type='button' icon="pi pi-plus"
            className='p-button-text p-button-rounded p-button-success p-0 m-0'
            onClick={(onAddDetail)}
        />
    </>;

    return (
        <form onSubmit={onSubmit}>
            <div  >
                <Spinner loading={request.isLoading} className="p-fluid" />

                <div className="p-fluid" >

                    <span className="p-float-label mt-5">
                        <AutoComplete inputId="seller" {...seller.field}
                            forceSelection
                            field='name'
                            suggestions={availableSeller}
                            completeMethod={onSearchSheller}
                        />
                        <label htmlFor="seller">Seller</label>
                    </span>
                    <small className='p-error'>{seller.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="name" {...name.field} type='text' />
                        <label htmlFor="name">Nombre</label>
                    </span>
                    <small className='p-error'>{name.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputTextarea id="description" {...description.field} style={{ resize: 'vertical', minHeight: "6rem" }} />
                        <label htmlFor="description">Descripción</label>
                    </span>
                    <small className='p-error'>{description.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <AutoComplete inputId="categories" {...categories.field}
                            multiple
                            forceSelection
                            suggestions={availableCategories}
                            completeMethod={onSearchCategories}
                        />
                        <label htmlFor="categories">Categorías</label>
                    </span>
                    <small className='p-error'>{categories.fieldState.error?.message}</small>

                    <Panel
                        ref={panelRef}
                        className='mt-3'
                        header={(<span>Details</span>)}
                        toggleable
                        collapseIcon="pi pi-arrow-down-left-and-arrow-up-right-to-center"
                        expandIcon="pi pi-arrow-up-right-and-arrow-down-left-from-center"
                        icons={iconAddDetail} >
                        <div>
                            {details.fields.map((ef, index) => {
                                return (
                                    <div>
                                        <div key={ef.id} className="grid gap-2 mt-5">
                                            <Controller control={control} name={`details.${index}.imageUrl`} defaultValue=''
                                                rules={{ required: { value: true, message: 'Image is required' } }}
                                                render={({ field }) => (
                                                    <div className="col-4 flex p-float-label">
                                                        <InputFile className='p-inputwrapper-filled'  {...field} label='Input File' accept="image/*" dataUrl={ef.imageUrl} onFileChanged={(e) => field.onChange(e.dataUrl)} />
                                                        <label className='p-filled' >Image</label>
                                                    </div>
                                                )}
                                            />
                                            <Controller control={control} name={`details.${index}.description`} defaultValue=''
                                                rules={{ required: { value: true, message: 'Description is required' } }}
                                                render={({ field }) => (
                                                    <div className='col flex px-0 p-float-label'>
                                                        <InputTextarea {...field}
                                                            className='p-filled'
                                                            style={{
                                                                resize: 'vertical',
                                                                minHeight: "6rem",
                                                                overflowY: "scroll",
                                                            }}
                                                        />
                                                        <label>Descripción</label>
                                                    </div>
                                                )}
                                            />
                                            <Button type='button' icon="pi pi-minus"
                                                className='p-button-text p-button-rounded p-button-danger p-0 ml-0 m-2'
                                                onClick={() => details.remove(index)}
                                            />

                                        </div>
                                        <small className='p-error'>{getFieldState(`details.${index}`).invalid && "Detail image and description is required"}</small>
                                    </div>
                                );
                            })}
                        </div>
                    </Panel>
                </div>

                <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                    {(request.errors?.map((e, i) => <p key={i} className='p-error p-0 m-0 text-sm'>{e.message}</p>))}
                </div>

                <div className='flex pt-3 justify-content-end'>
                    <Button type='submit' label="Send" icon="pi pi-check" />
                </div>
            </div >
        </form >
    );
}
