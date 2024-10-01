import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Checkbox } from 'primereact/checkbox';
import { createStoreEntryPoint } from '../core/api/Context';
import { useForm, useController } from "react-hook-form"
import { Spinner } from '../layout/Spinner';
import { useToast } from '../core/message/Context';
import { AutoComplete, AutoCompleteCompleteEvent } from 'primereact/autocomplete';
import { useState, useEffect } from 'react';
import { Dialog } from 'primereact/dialog';
import { roleListStore } from '../core/api/Stores';

type TUser = typeof requestStore.types.body
const requestStore = createStoreEntryPoint("/security/role", "patch")

export function ConfigRoleDialog({ item, onSuccess, onError, onHide }: {
    item: TUser,
    onSuccess?: () => void
    onError?: () => void
    onHide?: () => void
}) {
    return <>
        <Dialog
            visible={!!item}
            header={(item?.name ? `Config role ${item?.name}` : 'Create a new role')}
            onHide={onHide}
            style={{ width: 'max(800px, 50vw)' }}
            breakpoints={{ '960px': '100vw' }}>
            {item &&
                <ConfigRoleForm
                    item={item}
                    onSuccess={onSuccess}
                    onError={onError}
                />
            }
        </Dialog>
    </>
}

export function ConfigRoleForm({ item, onSuccess, onError }: {
    item: TUser,
    onSuccess?: () => void
    onError?: () => void
}) {
    const isEdition = item.name && item.name != '';
    const request = requestStore.use();
    const requestRoles = roleListStore.use()
    const toast = useToast()
    const [resourcesFiltered, setResourcesFiltered] = useState<string[]>([])
    const [actionsFiltered, setActionsFiltered] = useState<string[]>([])
    const [allowCreateResources, setAllowCreateResources] = useState(false)
    const [allowCreateActions, setAllowCreateActions] = useState(false)
    const { control, handleSubmit } = useForm<TUser>({
        defaultValues: {
            roleId: item.roleId ?? null,
            name: item.name ?? null,
            actions: item.actions,
            resources: item.resources,
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
            minLength: {
                value: isEdition ? 1 : 8,
                message: 'Name length must be at least 8 characters long'
            },
        }
    })
    const resources = useController({
        control,
        name: 'resources',
        rules: {
            validate: (e) => e?.length > 0 || 'At most one resource is required'
        }
    })
    const actions = useController({
        control,
        name: 'actions',
        rules: {
            validate: (e) => e?.length > 0 || 'At most one action is required'
        }
    })

    useEffect(() => {
        console.log("requestRoles done:", requestRoles.done)
        requestRoles.initialize({ query: { RowsPerPage: 10000 } })
    }, [])

    const onSearchResources = (event: AutoCompleteCompleteEvent) => {
        const items = [...new Set(requestRoles.data?.items?.flatMap(e => e.resources) ?? [])];
        if (!event.query.trim().length) {
            setResourcesFiltered(items);
        }
        else {
            let query = event.query.toLowerCase()
            let filtered = items.filter(c => c.toLowerCase().startsWith(query));

            if (allowCreateResources) {
                filtered.push(event.query);
            }
            setResourcesFiltered(filtered);
        }
    }


    const onSearchActions = (event: AutoCompleteCompleteEvent) => {
        const items = [...new Set(requestRoles.data?.items?.flatMap(e => e.actions) ?? [])];
        if (!event.query.trim().length) {
            setActionsFiltered(items);
        }
        else {
            let query = event.query.toLowerCase()
            let filtered = items.filter(c => c.toLowerCase().startsWith(query));

            if (allowCreateActions) {
                filtered.push(event.query);
            }
            setActionsFiltered(filtered);
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
                        <InputText id="name" {...name.field} autoComplete='off' type='text' readOnly={isEdition} />
                        <label htmlFor="name">Name</label>
                    </span>
                    <small className='p-error'>{name.fieldState.error?.message}</small>

                    <span className="p-float-label mt-5">
                        <AutoComplete inputId="resources" {...resources.field}
                            multiple
                            dropdown
                            forceSelection
                            suggestions={resourcesFiltered}
                            completeMethod={onSearchResources}
                        />
                        <label htmlFor="resources">Resources</label>
                    </span>
                    <small className='p-error'>{resources.fieldState.error?.message}</small>

                    <div className="flex align-items-center mt-3">
                        <Checkbox inputId="resourcesCheck" checked={allowCreateResources} onChange={e => setAllowCreateResources(e.checked)} className="mr-2"></Checkbox>
                        <label htmlFor='resourcesCheck'>Allow create resources</label>
                    </div>

                    <span className="p-float-label mt-5">
                        <AutoComplete inputId="actions" {...actions.field}
                            multiple
                            dropdown
                            forceSelection
                            suggestions={actionsFiltered}
                            completeMethod={onSearchActions}
                        />
                        <label htmlFor='actions'>Actions</label>
                    </span>
                    <small className='p-error'>{actions.fieldState.error?.message}</small>

                    <div className="flex align-items-center mt-3">
                        <Checkbox inputId="actionsCheck" checked={allowCreateActions} onChange={e => setAllowCreateActions(e.checked)} className="mr-2"></Checkbox>
                        <label htmlFor="actionsCheck">Allow create actions</label>
                    </div>

                    <div className='pt-3' hidden={request.errors === undefined || request.isLoading}>
                        {(request.errors?.map(e => <small key={e.code} className='p-error'>{e.message}</small>))}
                    </div>

                    <div className='flex justify-content-end'>
                        <div className='flex'>
                            <Button type='submit' label="Save" icon="pi pi-check" />
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
