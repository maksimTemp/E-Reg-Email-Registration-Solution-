import axios from 'axios';
import { useState } from 'react';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import CodeInput from './components/CodeInput';
import EmailInput from './components/EmailInput';
import SuccessPage from './components/SuccessPage';
import './styles.css';

function App() {
    const [step, setStep] = useState('email');
    const [message, setMessage] = useState('');
    const [email, setEmail] = useState('');
    const [code, setCode] = useState('');

    const handleEmailSubmit = async (email: string) => {
        try {
            const response = await axios.post('https://localhost:7051/EmailRegistration/RegisterEmail', { email });

            if (response.status === 200) {
                setEmail(email);
                setStep('code');
            }
            else {
                handleErrorResponse(response);
            }
        }
        catch (error) {
            toast.error('Failed to submit email');
        }
    };

    const handleCodeConfirm = async (inputCode: string) => {
        try {
            const response = await axios.post('https://localhost:7051/EmailRegistration/ValidateEmailCode', { email, code: inputCode });

            if (response.status === 200) {
                setMessage('Thank you for your registration');
                setStep('success');
            }
            else {
                handleErrorResponse(response);
            }
        }
        catch (error) {
            toast.error('Failed to confirm code');
        }
    };

    const handleErrorResponse = (response: ServerResponse) => {
        if (response.status === 400) {
            toast.error(`Bad Request: ${response.data}`);
        }
        else if (response.status === 500) {
            toast.error(`Internal Server Error: ${response.data}`);
        }
        else if (response.status === 429) {
            toast.error(`Too Many Requests: ${response.data}`);
        }
        else {
            toast.error(`An error occurred: ${response.data}`);
        }
    };

    return (
        <div className="App">
            <div className="card">
                {step === 'email' && <EmailInput onSubmit={handleEmailSubmit} />}
                {step === 'code' && <CodeInput onConfirm={handleCodeConfirm} email={email} />}
                {step === 'success' && <SuccessPage />}
            </div>
            <ToastContainer position="top-right" autoClose={5000} hideProgressBar />
        </div>
    );
}

interface ServerResponse {
    status: number;
    data: string;
}

export default App;

