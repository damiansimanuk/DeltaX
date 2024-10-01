import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { useForm, Controller } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { createStoreEntryPoint } from '../core/api/Context';

const requestStore = createStoreEntryPoint("/api/v1/Product/seller", "post");

export function ConfigSellerForm() {
    type TForm = typeof requestStore.types.body;
    const request = requestStore.use();
    const { control, handleSubmit } = useForm<TForm>()

    const onSubmit = handleSubmit(data => {
        console.log(JSON.stringify(data));
        request.fetch({ body: data })
    })

    const header = <div className='p-4'>
        <span className='font-bold text-2xl'>Registrar Empresa</span>
    </div>;

    const footer = <div className='flex justify-content-end'>
        <div className='flex'>
            <Button type='submit' label="Send" icon="pi pi-check" />
        </div>
    </div>;

    return (
        <form onSubmit={onSubmit}>
            <Card
                className="p-fluid mx-0 mt-0 sm:mt-5 sm:mx-auto relative"
                style={{ maxWidth: '900px' }}
                footer={footer}
                header={header}
            >
                <Spinner loading={request.isLoading} />

                <div className="p-fluid" >
                    <Controller control={control} name='name' defaultValue=''
                        rules={{
                            required: {
                                value: true,
                                message: 'Name is required'
                            },
                        }}
                        render={({ field, fieldState }) => (<>
                            <span className="p-float-label mt-5">
                                <InputText id={field.name} {...field} type='text' />
                                <label htmlFor={field.name}>Nombre</label>
                            </span>
                            <small className='p-error'>{fieldState.error?.message}</small>
                        </>)}
                    />

                    <Controller control={control} name='email' defaultValue=''
                        rules={{
                            required: {
                                value: true,
                                message: 'Email is required'
                            },
                            pattern: {
                                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                message: "Invalid email address"
                            }
                        }}
                        render={({ field, fieldState }) => (<>
                            <span className="p-float-label mt-5">
                                <InputText id={field.name} {...field} type='text' />
                                <label htmlFor={field.name}>Email</label>
                            </span>
                            <small className='p-error'>{fieldState.error?.message}</small>
                        </>)}
                    />

                    <Controller control={control} name='phoneNumber' defaultValue={undefined}
                        rules={{
                            required: {
                                value: true,
                                message: 'Phone number is required'
                            }
                        }}
                        render={({ field, fieldState }) => (<>
                            <span className="p-float-label mt-5">
                                <InputText id={field.name} {...field} type='text' />
                                <label htmlFor={field.name}>Phone number</label>
                            </span>
                            <small className='p-error'>{fieldState.error?.message}</small>
                        </>)}
                    />

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map((e, i) => <p key={i} className='p-error p-0 m-0 text-sm'>{e.message}</p>))}
                    </div>

                </div>
            </Card >
        </form>
    );
}
