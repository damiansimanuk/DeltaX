import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';
import { useForm, useController } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { useToast } from '../core/message/Context';
import { Link } from 'react-router-dom';
import { createStoreEntryPoint } from '../core/api/Store';

const requestStore = createStoreEntryPoint("/security/resetPassword", "post");

export function PageResetPasswordForm() {
    type TForm = typeof requestStore.types.body & { confirmPassword: string; }
    const request = requestStore.use();
    const { control, handleSubmit, getValues } = useForm<TForm>()
    const toast = useToast()

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
    const resetCode = useController({
        control,
        name: 'resetCode',
        rules: {
            required: {
                value: true,
                message: 'Reset code is required'
            }
        }
    })
    const newPassword = useController({
        control,
        name: 'newPassword',
        rules: {
            validate: (value) => value?.length > 6 || 'Passwords must be at least 6 characters',
        }
    })
    const confirmPassword = useController({
        control,
        name: 'confirmPassword',
        rules: {
            validate: (value) => value === getValues('newPassword') || 'Passwords do not match',
        }
    })

    const onSubmit = handleSubmit(data => {
        toast?.clear();
        request.fetch({ body: data })
            .then(() => toast.show({ severity: 'success', summary: 'Success', detail: 'Change password with success', life: 2000 }))
            .catch(e => toast.show({ severity: 'error', summary: e.errors[0]?.message ?? e.message, life: 30000 }))
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
                    <span className="p-float-label mt-5">
                        <InputText id="email" {...email.field} autoComplete='off' type='email' />
                        <label htmlFor="email">Email</label>
                    </span>
                    <small className='p-error'>{email.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="resetCode" {...resetCode.field} autoComplete='off' type='text' />
                        <label htmlFor="resetCode">Reset Code (token)</label>
                    </span>
                    <small className='p-error'>{resetCode.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <Password inputId="newPassword" {...newPassword.field} toggleMask feedback={false} autoComplete='off' />
                        <label htmlFor="newPassword">New Password</label>
                    </span>
                    <small className='p-error'>{newPassword.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <Password inputId="confirmPassword" {...confirmPassword.field} toggleMask feedback={false} autoComplete='off' />
                        <label htmlFor="confirmPassword">Confirm Password</label>
                    </span>
                    <small className='p-error'>{confirmPassword.fieldState.error?.message}</small>

                    <div className="flex justify-content-end mt-3  text-sm">
                        Ir a <Link className="no-underline cursor-pointer text-blue-300 px-1 font-medium" to='/security/login'>login</Link>
                    </div>
                </div>
            </Card >
        </form>
    );
}
