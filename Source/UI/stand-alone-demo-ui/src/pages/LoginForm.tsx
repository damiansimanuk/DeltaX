import { useState } from 'react';
import { Password } from 'primereact/password';
import { InputText } from 'primereact/inputtext';
import { Card } from 'primereact/card';
import { Button } from 'primereact/button';

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const header = <div className='p-4'><span className='font-bold text-2xl'>Login</span></div>;

    const footer = <div className='flex justify-content-end'>
        <div className='flex'>
            <Button label="Save" icon="pi pi-check" style={{ marginRight: '.25em' }} />
            <Button label="Cancel" icon="pi pi-times" className="p-button-secondary" />
        </div>
    </div>;


    // return (

    //     <>
    //         <div className="p-fluid mx-0 mt-0 sm:mt-5 sm:mx-auto sm:max-w-30rem p-card p-component"
    //             data-pc-name="card" data-pc-section="root">
    //             <div className="p-card-header" data-pc-section="header">
    //                 <div className="p-4">
    //                     <span className="font-bold text-2xl">Login</span>
    //                 </div>
    //             </div>
    //             <div className="p-card-body bg-blue-300" >
    //                 <div className="p-card-content bg-red-300 pt-0" data-pc-section="content">
    //                     <div>
    //                         <div className="p-fluid">
    //                             <span className="p-float-label w-full">
    //                                 <input className="p-inputtext p-component" id="username" data-pc-name="inputtext" data-pc-section="root" value="" />
    //                                 <label  >Username</label>
    //                             </span>
    //                         </div>
    //                         <div className="field p-0 pt-4">
    //                             <label> asdf </label>
    //                         </div>
    //                     </div>
    //                 </div>
    //                 <div className="p-card-footer" data-pc-section="footer">
    //                     <div className="flex justify-content-end">
    //                         <div className="flex">
    //                             <button aria-label="Save" className="p-button p-component" data-pc-name="button" data-pc-section="root">
    //                                 <span className="p-button-icon p-c p-button-icon-left pi pi-check" data-pc-section="icon">
    //                                 </span>
    //                                 <span className="p-button-label p-c" data-pc-section="label">Save</span>
    //                             </button>
    //                             <button aria-label="Cancel" className="p-button-secondary p-button p-component" data-pc-name="button" data-pc-section="root">
    //                                 <span className="p-button-icon p-c p-button-icon-left pi pi-times" data-pc-section="icon"></span>
    //                                 <span className="p-button-label p-c" data-pc-section="label">Cancel</span>
    //                             </button>
    //                         </div>
    //                     </div>
    //                 </div>
    //             </div>
    //         </div>
    //     </>
    // )

    return (
        <Card className="p-fluid mx-0 mt-0 sm:mt-5 sm:mx-auto max-w-screen-sm" footer={footer} header={header}>
            <div className="p-fluid">
                <span className="p-float-label w-full mt-5">
                    <InputText id="username" value={email} onChange={(e) => setEmail(e.target.value)} className='w-full' />
                    <label htmlFor="username">Username</label>
                </span>
                <span className="p-field w-full mt-5">
                    <Password inputId="password" value={password}
                        inputStyle={{ minWidth: '400px' }}
                        onChange={(e) => setPassword(e.target.value)} toggleMask className='w-full grid' />
                    <label htmlFor="password">Password</label>
                </span>
                <div className='field p-0 pt-4'>
                    <label> asdf </label>
                </div>
            </div>
        </Card >
    );
}