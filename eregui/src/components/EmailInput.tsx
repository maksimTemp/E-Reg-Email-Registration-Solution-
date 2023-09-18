import React, { useState } from 'react';

interface EmailInputProps {
    onSubmit: (email: string) => void;
}

function EmailInput({ onSubmit }: EmailInputProps) {
    const [email, setEmail] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(email);
    };

    return (
        <form className="registration-card" onSubmit={handleSubmit}>
            <label>Email:</label>
            <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
            />
            <button type="submit">Send Code</button>
        </form>
    );
}

export default EmailInput;
