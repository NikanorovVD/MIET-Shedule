import TeacherPair from "./TeacherPair";
import "./TeacherSection.css"
import { useCallback, useEffect, useState } from "react";
import useDate from "../../hooks/useDate";
import DatePicker from "./../DatePicker";

export default function TeacherSection() {
    const [shedule, setShedule] = useState()
    const [teacher, setTeacher] = useState('')
    const [teacherList, setTeacherList] = useState()
    const startDate = useDate()
    const endDate = useDate(new Date((new Date()).setDate((new Date()).getDate() + 7)))
    const [invalidTeacher, setInvalidTeacher] = useState(false)

    // запрос списка преподавателей
    const fetchTeacherList = useCallback(async () => {
        const teacherListResponse = await fetch("Teachers")
        if (teacherListResponse.status == 200) {
            const teachers = await teacherListResponse.json()
            setTeacherList(teachers)
        }
    }, [])

    // запрос расписания преподавателя
    const fetchShedule = useCallback(async () => {
        if (teacher.length != 0) {
            const formattedStartDate = new Date(startDate.value).toISOString().slice(0, 10)
            const formattedEndDate = new Date(endDate.value).toISOString().slice(0, 10)
            const sheduleResponse = await fetch(`Shedule/teacher/${teacher}?startDate=${formattedStartDate}&endDate=${formattedEndDate}`)

            if (sheduleResponse.status == 200) {
                const sheduleRecords = await sheduleResponse.json()
                setShedule(sheduleRecords)
            }
        }
    }, [teacher, startDate.value, endDate.value])

    // получение списка преподавателей при старте
    useEffect(() => {
        fetchTeacherList()
    }, [])

    // получение пар преподавателя при изменении ввода
    useEffect(() => {
        if (teacherList != undefined && teacherList.includes(teacher)) {
            fetchShedule()
            setInvalidTeacher(false)
        }
        else {
            setShedule(undefined)
            if (teacher.length != 0) {
                setInvalidTeacher(true)
            }
        }
    }, [teacher, startDate.value, endDate.value, teacherList])

    function keyExtractor(couple) {
        return `${couple.name}-${couple.date}-${couple.teacher}-${couple.order}-${couple.auditoriums}-${couple.groups}`;
    }

    return (
        <section className="teacher-section">
            {teacherList != undefined &&
                <>

                    <input
                        className={`teacher-input ${invalidTeacher ? "error-input" : ""}`}
                        type="text"
                        list="teachers"
                        value={teacher}
                        onChange={(event) => setTeacher(event.target.value)} />
                    <datalist id="teachers">
                        {teacherList.map(g => <option key={g}>{g}</option>)}
                    </datalist>
                    <DatePicker {...startDate} />
                    <DatePicker {...endDate} />
                </>
            }

            {shedule != undefined &&
                shedule.map(c =>
                    <TeacherPair key={keyExtractor(c)} {...c}></TeacherPair>
                )
            }

            {
                shedule != undefined && shedule.length == 0 &&
                <h2>Нет занятий</h2>
            }

        </section>
    )
}