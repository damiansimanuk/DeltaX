import { ChangeEventHandler, forwardRef, useEffect, useRef, useState } from 'react';
import { useElementSize } from '../core/hooks/useElementSize';
import mergeRefs from "../core/hooks/mergeRefs";
import './InputFile.css'

type TInputFile = {
    label: string,
    dataUrl?: string,
    accept?: string | undefined;
    className?: string | undefined;
    onBlur?: () => void
    onChange?: ChangeEventHandler<HTMLInputElement> | undefined;
    onFileChanged?: (e: { file: File, dataUrl: string }) => void | undefined;
};

export const InputFile = forwardRef<HTMLInputElement, TInputFile>((props, ref) => {
    const inputRef = useRef<HTMLInputElement>();
    const [file, setFile] = useState<File>()
    const [dataUrl, setDataUrl] = useState<string>()
    const elementSize = useElementSize<HTMLDivElement>({});

    console.log({ dataUrl, d: props.dataUrl })

    function readAsBase64(file: File) {
        var reader = new FileReader();
        reader.onload = () => {
            setDataUrl(`${reader.result}`);
        };
        reader.onerror = () => {
            setDataUrl(null);
        };
        if (file) {
            reader.readAsDataURL(file);
        }
        else {
            setDataUrl(null);
        }
    }

    useEffect(() => {
        readAsBase64(file)
    }, [file])

    useEffect(() => {
        if (props.onFileChanged) props.onFileChanged({ file, dataUrl: dataUrl })
    }, [dataUrl])

    useEffect(() => {
        setDataUrl(props.dataUrl)
    }, [props.dataUrl])

    async function onFileChanged(e: React.ChangeEvent<HTMLInputElement>) {
        e.preventDefault()
        const _file = e.target.files && e.target.files[0];
        if (!_file) {
            setFile(null)
        }
        else {
            setFile(_file)
        }
        if (props.onChange) props.onChange(e)
    }

    function onBtnDrop(e: React.DragEvent<HTMLDivElement>) {
        e.preventDefault();
        e.stopPropagation();
        const _file = e.dataTransfer.files && e.dataTransfer.files[0];
        if (!_file) {
            setFile(null)
        }
        else {
            setFile(_file)
        }
    }

    function preventDefault(e: React.MouseEvent<HTMLDivElement>) {
        e.preventDefault();
        e.stopPropagation();
    }

    return (
        <div
            ref={elementSize.ref}
            draggable='true'
            onClick={() => inputRef.current.click()}
            onDrop={onBtnDrop}
            onDrag={preventDefault}
            onDragStart={preventDefault}
            onDragEnd={preventDefault}
            onDragOver={preventDefault}
            onDragEnter={preventDefault}
            onDragLeave={preventDefault}
            style={{ cursor: 'pointer' }}
            className={'input-file relative flex w-full ' + (props.className ?? '')}
        >
            <input
                className='absolute'
                ref={mergeRefs(inputRef, ref)}
                type='file'
                accept={props.accept}
                onChange={onFileChanged}
                style={{ opacity: 0, width: 0, height: 0 }}
            />

            {dataUrl
                ? <img src={dataUrl}
                    style={{
                        width: elementSize.width,
                        maxHeight: elementSize.width,
                        objectFit: 'contain',
                    }}
                />
                : <div
                    className="empty-file flex justify-content-center align-items-center w-full p-2 border border-dashed border-2"
                >
                    Drop your image here
                </div>
            }
        </div>
    );
})