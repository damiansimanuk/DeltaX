import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { useEntryPoint } from '../api/context';
import { useForm, Controller, useFieldArray } from "react-hook-form"
import { Spinner } from '../layout/spinner';
import { AutoComplete, AutoCompleteCompleteEvent } from 'primereact/autocomplete';
import { useState } from 'react';
import { Panel } from 'primereact/panel';
import { InputFile } from '../components/input-file';
import { InputTextarea } from 'primereact/inputtextarea';


const categoriesCatalog = [
    'New York',
    'Rome',
    'London',
    'Istanbul',
    'Paris',
];


export function ConfigProductForm() {
    type TState = typeof request.types.body;
    const request = useEntryPoint("/Product/product", "post");
    const [categories, setCategories] = useState(categoriesCatalog);
    const { control, handleSubmit } = useForm<TState>()
    const details = useFieldArray({ control, name: "details" })

    const onSubmit = handleSubmit(data => {
        console.log(JSON.stringify(data));
        const categories = data.categories || [];
        const details = data.details || [];
        request.fetch({ body: { ...data, categories, details } })
    })

    const onAddDetail = handleSubmit(() => {
        details.append({ description: "", imageUrl: '' })
    })

    const onSearchCategories = (event: AutoCompleteCompleteEvent) => {
        setTimeout(() => {
            if (!event.query.trim().length) {
                setCategories(categoriesCatalog);
            }
            else {
                let query = event.query.toLowerCase()
                let filtered = categoriesCatalog.filter(c => c.toLowerCase().startsWith(query));
                filtered.push(event.query);
                setCategories(filtered);
            }
        }, 1050);
    }


    const header = <div className='p-4'>
        <span className='font-bold text-2xl'>Registrar Producto</span>
    </div>;

    const footer = <div className='flex justify-content-end'>
        <div className='flex'>
            <Button type='submit' label="Send" icon="pi pi-check" />
        </div>
    </div>;

    const iconAdd = <div className='flex justify-content-end'>
        <Button type='button' icon="pi pi-plus"
            className='p-button-text p-button-rounded p-button-success p-0 m-0'
            onClick={(onAddDetail)}
        />
    </div>;

    return (
        <form onSubmit={onSubmit}>
            <Card
                className="p-fluid mx-0 mt-0 sm:mt-5 sm:mx-auto relative"
                style={{ maxWidth: '900px' }}
                footer={footer}
                header={header}
            >
                <Spinner loading={request.isLoading} className="p-fluid" >

                    <Controller control={control} name='name' defaultValue=''
                        rules={{ required: true }}
                        render={({ field }) => (
                            <span className="p-float-label mt-5">
                                <InputText id={field.name} {...field} type='text' />
                                <label htmlFor={field.name}>Nombre</label>
                            </span>
                        )}
                    />

                    <Controller control={control} name='description' defaultValue=''
                        rules={{ required: true }}
                        render={({ field }) => (
                            <span className="p-float-label mt-5">
                                <InputText id={field.name} {...field} type='text' />
                                <label htmlFor={field.name}>Descripción</label>
                            </span>
                        )}
                    />

                    <Controller control={control} name='categories' defaultValue={undefined}
                        rules={{
                            required: true,
                            validate: (value) => value?.length >= 1 || 'Category is required',
                        }}
                        render={({ field }) => (
                            <span className="p-float-label mt-5">
                                <AutoComplete inputId={field.name} {...field}
                                    multiple
                                    forceSelection
                                    suggestions={categories}
                                    completeMethod={onSearchCategories} />
                                <label htmlFor={field.name}>Categorías</label>
                            </span>
                        )}
                    />

                    <Panel
                        className='mt-2'
                        header={(<span>Details</span>)}
                        icons={iconAdd} >
                        {details.fields.map((field, index) => {
                            return (
                                <div key={field.id} className="grid gap-2">
                                    <Controller control={control} name={`details.${index}.imageUrl`} defaultValue=''
                                        rules={{ required: true }}
                                        render={({ field }) => (
                                            <div className='col-4 flex'>
                                                <InputFile {...field} label='Input File' accept="image/*" onFileChanged={(e) => field.onChange(e.dataUrl)} />
                                            </div>
                                        )}
                                    />
                                    <Controller control={control} name={`details.${index}.description`} defaultValue=''
                                        rules={{ required: true }}
                                        render={({ field }) => (
                                            <div className='col px-0'>
                                                <span className="field">
                                                    <label htmlFor={field.name}>Descripción</label>
                                                    <InputTextarea id={field.name} {...field} style={{ resize: 'vertical', minHeight: "6rem" }} />
                                                </span>
                                            </div>
                                        )}
                                    />
                                    <Button type='button' icon="pi pi-minus"
                                        className='p-button-text p-button-rounded p-button-danger p-0 ml-0 m-2'
                                        onClick={() => details.remove(index)}
                                    />
                                </div>
                            );
                        })}
                    </Panel>

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map((e, i) => <p key={i} className='p-error p-0 m-0 text-sm'>{e.message}</p>))}
                    </div>

                </Spinner>
            </Card >
        </form>
    );
}
