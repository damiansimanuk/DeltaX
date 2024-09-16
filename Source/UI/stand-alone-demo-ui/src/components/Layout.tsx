

export default function SubWeatherForecast() {
    const api = useApi();

    return (
        <>

            <PageLayout containerWidth={args.containerWidth} padding={args.padding} rowGap={args.rowGap} columnGap={args.columnGap} sx={args.sx}>
                {args['Render header?'] ? <PageLayout.Header padding={args['Header.padding']} divider={{
                    narrow: args['Header.divider.narrow'],
                    regular: args['Header.divider.regular'],
                    wide: args['Header.divider.wide']
                }} hidden={{
                    narrow: args['Header.hidden.narrow'],
                    regular: args['Header.hidden.regular'],
                    wide: args['Header.hidden.wide']
                }}>
                    <Placeholder height={args['Header placeholder height']} label="Header" />
                </PageLayout.Header> : null}
                <PageLayout.Content width={args['Content.width']} padding={args['Content.padding']} hidden={{
                    narrow: args['Content.hidden.narrow'],
                    regular: args['Content.hidden.regular'],
                    wide: args['Content.hidden.wide']
                }}>
                    <Placeholder height={args['Content placeholder height']} label="Content" />
                </PageLayout.Content>
                {args['Render pane?'] ? <PageLayout.Pane position={{
                    narrow: args['Pane.position.narrow'],
                    regular: args['Pane.position.regular'],
                    wide: args['Pane.position.wide']
                }} width={args['Pane.width']} minWidth={args['Pane.minWidth']} sticky={args['Pane.sticky']} resizable={args['Pane.resizable']} padding={args['Pane.padding']} divider={{
                    narrow: args['Pane.divider.narrow'],
                    regular: args['Pane.divider.regular'],
                    wide: args['Pane.divider.wide']
                }} hidden={{
                    narrow: args['Pane.hidden.narrow'],
                    regular: args['Pane.hidden.regular'],
                    wide: args['Pane.hidden.wide']
                }}>
                    <Placeholder height={args['Pane placeholder height']} label="Pane" />
                </PageLayout.Pane> : null}
                {args['Render footer?'] ? <PageLayout.Footer padding={args['Footer.padding']} divider={{
                    narrow: args['Footer.divider.narrow'],
                    regular: args['Footer.divider.regular'],
                    wide: args['Footer.divider.wide']
                }} hidden={{
                    narrow: args['Footer.hidden.narrow'],
                    regular: args['Footer.hidden.regular'],
                    wide: args['Footer.hidden.wide']
                }}>
                    <Placeholder height={args['Footer placeholder height']} label="Footer" />
                </PageLayout.Footer> : null}
            </PageLayout>
        </>
    );
}