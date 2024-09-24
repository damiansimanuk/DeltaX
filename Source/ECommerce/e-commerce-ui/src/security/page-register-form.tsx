import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { useEntryPoint } from '../api/context';
import { useForm, Controller } from "react-hook-form"
import { Spinner } from '../layout/spinner';


export function PageRegisterForm() {
    type TState = typeof request.types.body & { confirmPassword: string; }
    const request = useEntryPoint("/security/register", "post");
    const { control, handleSubmit, getValues } = useForm<TState>()

    const onSubmit = handleSubmit(data => {
        request.fetch({ body: { email: data.email, password: data.password } })
    })

    const header = <div className='p-4'><span className='font-bold text-2xl'>Register</span></div>;

    const footer = <div className='flex justify-content-end'>
        <div className='flex'>
            <Button type='submit' label="Send" icon="pi pi-check" />
        </div>
    </div>;

    return (
        <form onSubmit={onSubmit}>
            <Card className="p-fluid mx-0 mt-0 sm:mt-5 sm:mx-auto sm:max-w-30rem relative" footer={footer} header={header}>

                <Spinner loading={request.isLoading} />

                <div className="p-fluid">
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
                                <InputText id={field.name} {...field} autoComplete='off' type='email' />
                                <label htmlFor="email">Email</label>
                            </span>
                            <small className='p-error' hidden={!fieldState.invalid}>{fieldState.error?.message}</small>
                        </>)}
                    />

                    <Controller control={control} name='password' defaultValue=''
                        rules={{
                            required: {
                                value: true,
                                message: 'Password is required'
                            },
                            minLength: 2,
                        }}
                        render={({ field, fieldState }) => (<>
                            <span className="p-float-label mt-5">
                                <Password inputId={field.name} {...field} toggleMask feedback={false} />
                                <label htmlFor="password">Password</label>
                            </span>
                            <small className='p-error' hidden={!fieldState.invalid}>{fieldState.error?.message}</small>
                        </>)}
                    />

                    <Controller control={control} name='confirmPassword' defaultValue=''
                        rules={{
                            required: {
                                value: true,
                                message: 'Password is required'
                            },
                            validate: (value) => value === getValues('password') || 'Passwords do not match',
                            minLength: 2,
                        }}
                        render={({ field, fieldState }) => (<>
                            <span className="p-float-label mt-5">
                                <Password inputId={field.name} {...field} toggleMask feedback={false} />
                                <label htmlFor="confirmPassword">Confirm Password</label>
                            </span>
                            <small className='p-error' hidden={!fieldState.invalid}>{fieldState.error?.message}</small>
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
