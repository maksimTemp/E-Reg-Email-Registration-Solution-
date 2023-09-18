import React, { useState } from 'react';

interface CodeInputProps {
    onConfirm: (code: string) => void;
    email: string;
}

function CodeInput({ onConfirm, email }: CodeInputProps) {
    const [code, setCode] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onConfirm(code);
    };

    return (
        <form className="registration-card" onSubmit={handleSubmit}>
            <p>Write the code that was emailed to you:</p>
            <label className="email-address">{email}</label>
            <input
                type="text"
                value={code}
                onChange={(e) => setCode(e.target.value)}
                required
            />
            <button type="submit">Sumbit</button>
        </form>
    );
}

export default CodeInput;
