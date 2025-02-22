import "./DatePicker.css"

export default function DatePicker({ date, minusDay, addDay, setDate, ...props }) {
    return (
        <div className="datapicker-container" {...props}>
            <button className="triangle-buttons" onClick={() => minusDay()}>
                <div className="triangle-buttons__triangle triangle-buttons__triangle--l"></div>
            </button>
            <input type="date"
                value={date}
                onChange={(event) => setDate(event.target.value)}>
            </input>
            <button className="triangle-buttons" onClick={() => addDay()}>
                <div className="triangle-buttons__triangle triangle-buttons__triangle--r"></div>
            </button>
        </div>
    )
}