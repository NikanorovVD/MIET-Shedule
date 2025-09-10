import "./TeacherPair.css"

export default function TeacherPair({ order, name, teacher, time, auditoriums, date, groups, ...props }) {
    const parsedDate = new Date(date)
    const zeroPad = (num, places) => String(num).padStart(places, '0')
    const day = zeroPad(parsedDate.getDate(), 2)
    const month = zeroPad(parsedDate.getMonth() + 1, 2)
    return (
        <table {...props} className="teacher-pair">
            <tbody>
                <tr>
                    <td className="teacher-date">{`${day}.${month}`}</td>
                    <td rowSpan={2} className="teacher-name">{name}
                        <div>
                            <p className="teacher-group">{groups.join(", ")}</p>
                            <p className="teacher-teacher">{teacher}</p>
                            <p className="teacher-auditorium">{auditoriums.join(", ")}</p>
                            <p className="teacher-time">{time.start.slice(0, 5)}-{time.end.slice(0, 5)}</p>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td className="teacher-order">{order}</td>
                </tr>
            </tbody>
        </table>
    )
}