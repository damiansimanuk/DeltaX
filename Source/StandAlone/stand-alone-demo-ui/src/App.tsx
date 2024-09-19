import Router from "./Router";
import { PrimeReactProvider } from 'primereact/api';
import { ApiProvider } from './api/context';


export default function App() {
  return (
    <PrimeReactProvider >
      <ApiProvider baseUrl="http://localhost:5189" useCookies={true}>
        <Router />
      </ApiProvider>
    </PrimeReactProvider>
  );
}

