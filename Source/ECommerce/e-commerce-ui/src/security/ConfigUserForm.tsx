import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Checkbox } from 'primereact/checkbox';
import { useForm, useController } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { useToast } from '../core/message/Context';
import { AutoComplete, AutoCompleteCompleteEvent } from 'primereact/autocomplete';
import { useState, useEffect } from 'react';
import { Dialog } from 'primereact/dialog';
import { createStoreEntryPoint } from '../core/api/Store';
import { roleListStore, userListStore } from '../core/api/Shared';
import { Tag } from 'primereact/tag';

type TUser = typeof userListStore.types.result["items"][0]
type TUserForm = typeof requestStore.types.body
const requestStore = createStoreEntryPoint("/security/user", "put")

export function ConfigUserDialog({ item, onSuccess, onError, onHide }: {
    item: TUser,
    onSuccess?: () => void
    onError?: () => void
    onHide?: () => void
}) {
    return <>
        <Dialog
            visible={!!item}
            header={(item?.userName ? `Config user ${item?.userName}` : 'Create a new user')}
            onHide={onHide}
            style={{ width: 'max(800px, 50vw)' }}
            breakpoints={{ '960px': '100vw' }}>

            <ConfigUserForm
                item={item ?? {}}
                onSuccess={onSuccess}
                onError={onError}
            />
        </Dialog>
    </>
}

export function ConfigUserForm({ item, onSuccess, onError }: {
    item: TUser,
    onSuccess?: () => void
    onError?: () => void
}) {
    const isEdition = item.email && item.email != '';
    const request = requestStore.use();
    const requestRoles = roleListStore.use()
    const toast = useToast()
    const [allowCreateRole, setAllowCreateRole] = useState(false)
    const [rolesFiltered, setRolesFiltered] = useState([]);
    const { control, handleSubmit } = useForm<TUserForm>({
        defaultValues: {
            email: item.email,
            fullName: item.fullName ?? null,
            phoneNumber: item.phoneNumber ?? "123",
            roles: item.roles,
        }
    })
    const email = useController({
        control,
        name: 'email',
        rules: isEdition ? undefined : {
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
    const fullName = useController({
        control,
        name: 'fullName',
        rules: {
            minLength: 2,
        }
    })
    const phoneNumber = useController({
        control,
        name: 'phoneNumber',
        rules: {
            minLength: 2,
        }
    })
    const roles = useController({
        control,
        name: 'roles',
        rules: {
            validate: (e) => e?.length > 0 || 'Role is required'
        }
    })

    useEffect(() => {
        requestRoles.initialize({ query: { RowsPerPage: 10000 } })
    }, [])

    const onSearchRoles = (event: AutoCompleteCompleteEvent) => {
        const items = requestRoles.data?.items ?? [];
        if (!event.query.trim().length) {
            setRolesFiltered(items.map(r => r.name));
        }
        else {
            let query = event.query.toLowerCase()
            let filtered = items
                .map(r => r.name)
                .filter(c => c.toLowerCase().startsWith(query));

            if (allowCreateRole) {
                filtered.push(event.query);
            }
            setRolesFiltered(filtered);
        }
    }

    const onSubmit = handleSubmit(data => {
        toast?.clear()
        request.fetch({ body: data })
            .then(() => {
                toast.show({ severity: 'success', summary: "success", life: 5000 })
                onSuccess?.()
            })
            .catch((e) => {
                toast.show({ severity: 'error', summary: e.errors[0]?.message ?? e.message, life: 30000 })
                onError?.()
            })
    })

    return (
        <form onSubmit={onSubmit}>
            <div>

                <Spinner loading={request.isLoading} debounce={100} debounceStop={100} />

                <div className="p-fluid  ">
                    <span className="p-float-label mt-5">
                        <InputText id="email" {...email.field} autoComplete='off' type='email' readOnly={isEdition} />
                        <label htmlFor="email">Email</label>
                    </span>
                    <small className='p-error'>{email.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="fullName" {...fullName.field} autoComplete='off' type='text' />
                        <label htmlFor="fullName">Full Name</label>
                    </span>
                    <small className='p-error'>{fullName.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <InputText id="phoneNumber" {...phoneNumber.field} autoComplete='off' type='tel' />
                        <label htmlFor="phoneNumber">Phone Number</label>
                    </span>
                    <small className='p-error'>{phoneNumber.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <AutoComplete inputId="roles" {...roles.field}
                            dropdown
                            multiple
                            forceSelection
                            suggestions={rolesFiltered}
                            completeMethod={onSearchRoles} />
                        <label htmlFor="roles">Roles</label>
                    </span>
                    <small className='p-error'>{roles.fieldState.error?.message}</small>

                    <div className="flex align-items-center mt-3">
                        <Checkbox inputId="roleCheck" checked={allowCreateRole} onChange={e => setAllowCreateRole(e.checked)} className="mr-2"></Checkbox>
                        <label htmlFor="roleCheck">Allow create role</label>
                    </div>

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map(e => <small key={e.code} className='p-error'>{e.message}</small>))}
                    </div>

                    <div className='flex pt-3 justify-content-end'>
                        <div className='flex'>
                            <Button type='submit' label="Save" icon="pi pi-check" />
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
