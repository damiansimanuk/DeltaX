import { createRef, useLayoutEffect, useState } from "react";


export function useElementSize<T extends Element>(options: {
    minHeight?: number;
    minWidth?: number;
    offsetHeight?: number;
    offsetWidth?: number;
}) {
    options = { minHeight: 0, minWidth: 0, offsetHeight: 0, offsetWidth: 0, ...options }
    const ref = createRef<T>()
    const [height, setHeight] = useState(`${options.minHeight}px`)
    const [width, setWidth] = useState(`${options.minWidth}px`)

    useLayoutEffect(() => {
        const observer = new ResizeObserver(entries => {
            const rec = entries[0].contentRect
            const height = rec.height - options.offsetHeight
            const width = rec.width - options.offsetWidth
            setHeight(`${height > options.minHeight ? height : options.minHeight}px`)
            setWidth(`${width > options.minWidth ? width : options.minWidth}px`)
        })
        ref.current && observer.observe(ref.current)
        return () => ref.current && observer.unobserve(ref.current)
    }, []);

    return { ref, height, width }
}


