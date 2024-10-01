import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { Checkbox } from 'primereact/checkbox';
import { useForm, useController } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { Link, useNavigate } from 'react-router-dom';
import { useToast } from '../core/message/Context';
import { loginStore } from '../core/api/Stores';

type TForm = {
    email: string
    password: string
    rememberMe: boolean
}

export function PageLoginForm() {
    const request = loginStore.use();
    const navigate = useNavigate()
    const toast = useToast()
    const { control, handleSubmit } = useForm<TForm>()
    const { field: emailField, fieldState: emailState } = useController({
        control,
        name: 'email',
        defaultValue: 'user@test.com',
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
    const { field: passwordField, fieldState: passwordState } = useController({
        control,
        name: 'password',
        defaultValue: 'Pa$$word123',
        rules: {
            required: {
                value: true,
                message: 'Password is required'
            },
            minLength: 2,
        }
    })
    const { field: rememberMeField } = useController({
        control,
        name: 'rememberMe'
    })

    const onSubmit = handleSubmit(data => {
        toast?.clear()
        request.fetch({ body: { email: data.email, password: data.password }, query: { useCookies: true } })
            .then(() => {
                toast.show({ severity: 'success', summary: "success sig-in", life: 2000 })
                navigate("/")
            })
            .catch(e => toast.show({ severity: 'error', summary: e.message ?? e[0].message, life: 30000 }))
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

                <Spinner loading={request.isLoading} />

                <div className="p-fluid  ">
                    <span className="p-float-label mt-5">
                        <InputText id={emailField.name} {...emailField} autoComplete='off' type='email' />
                        <label htmlFor="email">Email</label>
                        <small className='p-error'>{emailState.error?.message}</small>
                    </span>

                    <span className="p-float-label mt-5">
                        <Password inputId={passwordField.name} {...passwordField} toggleMask feedback={false} />
                        <label htmlFor="password">Password</label>
                        <small className='p-error' >{passwordState.error?.message}</small>
                    </span>

                    <div className="flex align-items-center justify-content-between mt-3">
                        <div className="flex align-items-center">
                            <Checkbox inputId={rememberMeField.name} checked={rememberMeField.value} {...rememberMeField} className="mr-2"></Checkbox>
                            <label htmlFor="rememberMe">Recordar</label>
                        </div>
                        <a className="no-underline ml-2 text-right cursor-pointer text-sm text-blue-300 font-medium">
                            Recuperar contraseña
                        </a>
                    </div>

                    <div className="flex justify-content-end mt-3  text-sm">
                        ¿Desea
                        <Link className="no-underline cursor-pointer text-blue-300 px-1 font-medium" to='/register'>registrarse</Link>
                        con una nueva cuenta?
                    </div>

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map(e => <small key={e.code} className='p-error'>{e.message}</small>))}
                    </div>
                </div>
            </Card >
        </form>
    );
}
