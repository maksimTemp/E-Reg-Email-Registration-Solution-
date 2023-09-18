import React, { useState } from 'react';
import EmailInput from './components/EmailInput';
import CodeInput from './components/CodeInput';
import SuccessPage from './components/SuccessPage';
import './styles.css';
import axios from 'axios';

function App() {
    const [step, setStep] = useState('email');
    const [message, setMessage] = useState('');
    const [email, setEmail] = useState('');
    const [code, setCode] = useState('');

    const handleEmailSubmit = async (email: string) => {
        try {
            //const response = await axios.post('https://localhost:7051/y', { email });

            if (/*response.status === 200*/true) {
                setEmail(email);
                setStep('code');
            } else {
                // Обработка ошибки
            }
        } catch (error) {

        }
    };

    const handleCodeConfirm = async (inputCode: string) => {
        try {
            const response = await axios.post('https://localhost:7051/EmailRegistration/ValidateEmailCode', { email, code: inputCode });

            if (response.status === 200) {
                setMessage('Спасибо за регистрацию');
                setStep('success');
            } else {
                setMessage('Неверный код. Повторите попытку.');

            }
        } catch (error) {

        }
    };

    return (
        <div className="App">
            <div className="card">
                {step === 'email' && <EmailInput onSubmit={handleEmailSubmit} />}
                {step === 'code' && <CodeInput onConfirm={handleCodeConfirm} email={email} />}
                {step === 'success' && <SuccessPage />}
            </div>
        </div>
    );
}

export default App;

