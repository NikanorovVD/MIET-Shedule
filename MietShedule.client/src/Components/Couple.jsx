import "./Couple.css"

export default function Couple({ order, name, teacher, time, auditorium, date, group, ...props }) {
    const parsedDate = new Date(date)
    const zeroPad = (num, places) => String(num).padStart(places, '0')
    const day = zeroPad(parsedDate.getDate(), 2)
    const month = zeroPad(parsedDate.getMonth() + 1, 2)
    return (
        <table {...props} className="couple">
            <tbody>
                <tr>
                    <td className="date">{`${day}.${month}`}</td>
                    <td rowSpan={2} className="name">{name}
                        <div>
                            <p className="group">{group.join(", ")}</p>
                            <p className="teacher">{teacher}</p>
                            <p className="auditorium">{auditorium.join(", ")}</p>
                            <p className="time">{time}</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td className="order">{order}</td>

                </tr>
            </tbody>
        </table>
    )
}