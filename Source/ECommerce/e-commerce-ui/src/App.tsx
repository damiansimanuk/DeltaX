import Router from "./Router";
import { PrimeReactProvider } from 'primereact/api';
import { MessageProvider } from "./core/message/Context";


export default function App() {
  return (
    <PrimeReactProvider >
      <MessageProvider>
        <Router />
      </MessageProvider>
    </PrimeReactProvider>
  );
}

