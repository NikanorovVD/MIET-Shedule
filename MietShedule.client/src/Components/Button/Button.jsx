import './Button.css'

export default function Button({ children, active, ...props }) {
    return <button
        {...props}
        className={active ? 'button active' : 'button'}
    >
        {children}
    </button>
}