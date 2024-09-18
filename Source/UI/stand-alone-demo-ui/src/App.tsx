import Router from "./Router";
import { PrimeReactProvider } from 'primereact/api';
import { ApiProvider } from './api/context';
import Tailwind from 'primereact/passthrough/tailwind';


export default function App() {
  return (
    <PrimeReactProvider value={{ unstyled: true, pt: Tailwind }}>
      <ApiProvider baseUrl="http://localhost:5189" useCookies={true}>
        <Router />
      </ApiProvider>
    </PrimeReactProvider>
  );
}

