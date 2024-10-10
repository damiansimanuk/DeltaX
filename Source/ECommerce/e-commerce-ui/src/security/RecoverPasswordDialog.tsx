import { Dialog } from "primereact/dialog";
import { createStoreEntryPoint } from "../core/api/Store";
import { Link } from "react-router-dom";
import { useController, useForm } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { useEffect, useState } from "react";
import { useToast } from "../core/message/Context";
import { Button } from "primereact/button";

type TBody = typeof forgotPasswordStore.types.body
const forgotPasswordStore = createStoreEntryPoint("/security/forgotPassword2", "post")

export function RecoverPasswordDialog() {
    const request = forgotPasswordStore.use();
    const [visible, setVisible] = useState(false);
    const toast = useToast()
    const { control, handleSubmit } = useForm<TBody>()
    const email = useController({
        control,
        name: 'email',
        defaultValue: '',
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

    useEffect(() => {
        request.reset()
        email.field.onChange("")
    }, [])

    const onSubmit = handleSubmit(data => {
        toast?.clear();
        request.fetch({ body: data })
            .then((r) => showTooltip(r))
            .catch(e => toast.show({ severity: 'error', summary: e.message ?? e[0].message, life: 30000 }))
    })

    const showTooltip = (data: string) => {
        if (data) {
            toast.clear()
            navigator.clipboard.writeText(data)
            toast.show({ severity: 'success', summary: 'Recover code copied to clipboard', life: 30000 })
        }
    }

    return <>
        <a className="no-underline ml-2 text-right cursor-pointer text-sm text-blue-300 font-medium" onClick={() => setVisible(true)}>
            Recuperar contraseña
        </a>

        <Dialog
            visible={visible}
            header="Recover password"
            onHide={() => setVisible(false)}
            style={{ width: 'min(480px, 50vw)' }}
            breakpoints={{ '480px': '100vw' }}>
            <form >
                <div className="p-fluid"  >
                    <span className="p-float-label mt-5">
                        <InputText id="email" {...email.field} autoComplete='off' type='email' />
                        <label htmlFor="email">Email</label>
                    </span>
                    <small className='p-error'>{email.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="recoverCode"
                            autoComplete='off' type='text'
                            value={request.data ?? 'no code'}
                            readOnly className="text-yellow-200"
                            onClick={() => showTooltip(request.data)}
                        />
                        <label htmlFor="recoverCode">Recover Code (token)</label>
                    </span>

                    <div className="flex justify-content-end mt-3  text-sm">
                        Got to
                        <Link className="no-underline cursor-pointer text-blue-300 px-1 font-medium" to='/security/resetPassword'>Reset Password</Link>
                    </div>

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map(e => <small key={e.code} className='p-error'>{e.message}</small>))}
                    </div>

                    <div className='pt-3 flex justify-content-end'>
                        <div className='flex'>
                            <Button type='button' label="Get Code" icon="pi pi-check" onClick={onSubmit} />
                        </div>
                    </div>
                </div>
            </form>
        </Dialog>
    </>;
}
