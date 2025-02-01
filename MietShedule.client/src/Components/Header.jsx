import { useState } from 'react'
import logo from '/vite.svg'

export default function Header() {
    const [now, setNow] = useState(new Date())
    setInterval(() => setNow(new Date), 1000)
    const name = 'image'
    return (
        <header>
            <img src={logo} alt={name} />
            <h3>React Test</h3>
            <span>Текущее время: {now.toLocaleTimeString()}</span>
        </header>
    )
}