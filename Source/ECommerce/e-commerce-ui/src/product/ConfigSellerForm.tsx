import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { useForm, useController } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { createStoreEntryPoint } from '../core/api/Store';

const requestStore = createStoreEntryPoint("/api/v1/Product/seller", "post");
type TForm = typeof requestStore.types.body;

export function ConfigSellerForm({ item, onSuccess, onError }: {
    item: TForm,
    onSuccess?: () => void
    onError?: () => void
}) {
    const request = requestStore.use();
    const { control, handleSubmit } = useForm<TForm>({ defaultValues: item })

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

    const email = useController({
        control,
        name: 'email',
        rules: {
            required: {
                value: true,
                message: 'Email is required'
            },
            pattern: {
                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                message: "Invalid email address"
            }
        }
    })

    const phoneNumber = useController({
        control,
        name: 'phoneNumber',
        rules: {
            required: {
                value: true,
                message: 'Phone number is required'
            }
        }
    })

    const onSubmit = handleSubmit(data => {
        console.log(JSON.stringify(data));
        request.fetch({ body: data })
            .then(() => onSuccess?.())
            .catch(() => onError?.())
    })

    return (
        <form onSubmit={onSubmit}>
            <div>
                <Spinner loading={request.isLoading} />

                <div className="p-fluid" >

                    <span className="p-float-label mt-5">
                        <InputText id="name" {...name.field} type='text' />
                        <label htmlFor="name">Nombre</label>
                    </span>
                    <small className='p-error'>{name.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="email" {...email.field} type='text' />
                        <label htmlFor="email">Email</label>
                    </span>
                    <small className='p-error'>{email.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="phoneNumber" {...phoneNumber.field} type='text' />
                        <label htmlFor="phoneNumber">Phone number</label>
                    </span>
                    <small className='p-error'>{phoneNumber.fieldState.error?.message}</small>

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map((e, i) => <p key={i} className='p-error p-0 m-0 text-sm'>{e.message}</p>))}
                    </div>

                    <div className='flex pt-3 justify-content-end'>
                        <div className='flex'>
                            <Button type='submit' label="Send" icon="pi pi-check" />
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
