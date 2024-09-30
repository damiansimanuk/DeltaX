
export type OmitNever<T> = { [K in keyof T as T[K] extends undefined | never ? never : K]: T[K] }
export type StatusSuccess<T> = { [K in keyof T as K extends 200 | 201 | 202 | 203 | 204 | 205 | 206 ? K : never]: T[K] }
export type StatusError<T> = { [K in keyof T as K extends 400 | 401 | 403 | 404 | 500 ? K : never]: T[K] }

// npx openapi-typescript swagger.json --output swagger.ts
// npx openapi-typescript http://localhost:5299/swagger/v1/swagger.json --output swagger.ts


export function RequestTypeBuilder<Paths>() {
    return function <
        Url extends keyof Paths,
        Method extends keyof Omit<OmitNever<Paths[Url]>, "parameters">
    >(_url: Url, _method: Method) {
        type P = Required<Paths[Url][Method]>
        type Param = Required<Extract<P, { parameters: any }>['parameters']>
        type Resp = StatusSuccess<Extract<P, { responses: any }>['responses']>
        type Result = { [P in keyof Resp as keyof Resp[P]]: Resp[P]['content']['application/json']; }['content']
        type Path = Extract<Param, { path: any }>['path']
        type Query = Extract<Param, { query: any }>['query']
        type Body = Extract<Required<P>, { requestBody: { content: { 'application/json': any } } }>['requestBody']['content']['application/json']
        type O = { path: Path; query: Query; body: Body; }
        type Option = Pick<O, keyof OmitNever<O>>

        return {
            resultStatus: {} as keyof Resp,
            result: {} as Result,
            path: {} as Path,
            query: {} as Query,
            body: {} as Body,
            options: {} as Option,
        }
    }
}

export function RequestBuilder<Paths>(caller: ((url: string, method: string, body: string | null) => Promise<any>) | null = null) {

    function entryPoint<
        Url extends keyof Paths,
        Method extends keyof Omit<OmitNever<Paths[Url]>, "parameters">
    >(url: Url, method: Method) {
        const e = RequestTypeBuilder<Paths>()(url, method);
        type Option = typeof e.options;
        type Result = typeof e.result;
        type Path = typeof e.path;
        type Query = typeof e.query;
        type Body = typeof e.body;
        type Resp = typeof e.resultStatus;

        function fetch(options: Option): Promise<Result> {
            return call(caller, options)
        }

        async function call(caller: ((url: string, method: string, body: string | null) => Promise<Result>) | null, options: Option): Promise<Result> {
            let path = (options as any).path as Path;
            let query = (options as any).query as Query;
            let body = (options as any).body as Body;

            if (caller == null) {
                throw new Error("Invalid caller method")
            }

            let parsedUrl = `${url as string}`;
            if (path) {
                Object.keys(path || {}).forEach((p) => {
                    parsedUrl = parsedUrl.replace(`{${p}}`, `${path[p]}`);
                });
            }
            const queryParams = new URLSearchParams(query).toString();
            const parsedQuery =
                queryParams.length > 0 && !queryParams.startsWith('null')
                    ? '?' + queryParams
                    : '';
            parsedUrl = parsedUrl + parsedQuery;

            try {
                var res = await caller(parsedUrl, method, body)
                return res as Result
            }
            catch (e) {
                throw e
            }
        }

        return {
            call,
            fetch,
            method: method as string,
            url: url as string,
            types: {
                resultStatus: {} as Resp,
                result: {} as Result,
                path: {} as Path,
                query: {} as Query,
                body: {} as Body,
                options: {} as Option,
            }
        };
    }

    return { entryPoint };
}
