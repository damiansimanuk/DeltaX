import Router from "./Router";
import { PrimeReactProvider } from 'primereact/api';
import { ApiProvider } from './api/context';
import "primereact/resources/themes/lara-dark-cyan/theme.css";

export default function App() {
  return (
    <PrimeReactProvider>
      <ApiProvider baseUrl="http://localhost:5189" useCookies={true}>
        <Router />
      </ApiProvider>
    </PrimeReactProvider>
  );
}

