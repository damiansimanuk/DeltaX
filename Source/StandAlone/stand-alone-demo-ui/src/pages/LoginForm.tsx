import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { Checkbox } from 'primereact/checkbox';
import { useApi } from '../api/context';
import { useForm, Controller } from "react-hook-form"
import { Spinner } from '../layout/spinner';

type TState = {
    email: string
    password: string
    rememberMe: boolean
}

export default function Login() {
    const api = useApi();
    const { control, handleSubmit } = useForm<TState>()

    const onSubmit = handleSubmit(data => {
        api.auth.login({ email: data.email, password: data.password })
    })

    const header = <div className='p-4'><span className='font-bold text-2xl'>Login</span></div>;

    const footer = <div className='flex justify-content-end'>
        <div className='flex'>
            <Button type='submit' label="Login" icon="pi pi-check" />
        </div>
    </div>;

    return (
        <form onSubmit={onSubmit}>
            <Card className="p-fluid mx-0 mt-0 sm:mt-5 sm:mx-auto sm:max-w-30rem relative" footer={footer} header={header}>

                <Spinner loading={api.auth.isLoading} />

                <div className="p-fluid  ">
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
                        }} render={({ field, fieldState }) => (<>
                            <span className="p-float-label mt-5">
                                <InputText id={field.name} {...field} autoComplete='off' type='email' />
                                <label htmlFor="email">Email</label>
                            </span>
                            <small className='p-error' hidden={!fieldState.invalid}>{fieldState.error?.message}</small>
                        </>)} />

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
                        </>)} />


                    <div className="flex align-items-center justify-content-between mt-3">
                        <div className="flex align-items-center">
                            <Controller name='rememberMe' control={control} render={({ field }) => (
                                <Checkbox inputId={field.name} checked={field.value} {...field} className="mr-2"></Checkbox>
                            )} />
                            <label htmlFor="rememberMe">Recordar</label>
                        </div>
                        <a className="no-underline ml-2 text-right cursor-pointer text-sm text-blue-300 font-medium">
                            Recuperar contraseña
                        </a>
                    </div>

                    <div className="flex justify-content-end mt-3  text-sm">
                        ¿Desea 
                        <a className="no-underline cursor-pointer text-blue-300 px-1 font-medium">registrarse</a>
                        con una nueva cuenta? 
                    </div>

                    <div className='pt-3' hidden={api.auth.error === undefined || api.auth.isLoading}>
                        <small className='p-error'>{api.auth.error ?? ""}</small>
                    </div>
                </div>
            </Card >
        </form>
    );
}
